using LuckyExpenses.Application.Features.Authentication.Command.Login;
using LuckyExpenses.Application.Features.Authentication.Command.Logout;
using LuckyExpenses.Application.Features.Authentication.Command.Refresh;
using LuckyExpenses.Application.Features.Authentication.Command.Register;
using LuckyExpenses.Application.Interfaces.Authentication;
using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Enums;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace LuckyExpenses.Infrastructure.Authentication
{
    public class AuthenticationService(
        IUserRepository userRepository,
        IHasherService hasherService,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> jwtOptions) : IAuthenticationService
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default)
        {
            var exists = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
            if (exists is not null)
                throw new ConflictException("El correo electrónico ya está registrado");

            var user = new User
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PasswordHash = hasherService.Hash(command.Password),
                Role = UserRoleEnum.USER,
                State = UserStateEnum.ACTIVE
            };

            await userRepository.AddAsync(user, cancellationToken);

            try
            {
                await unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (IsUniqueViolation(ex))
            {
                throw new ConflictException("El correo electrónico ya está registrado", ex);
            }
        }

        public async Task<LoginResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken)
                ?? throw new InvalidCredentialsException("Credenciales inválidas");

            if (!hasherService.Verify(command.Password, user.PasswordHash))
                throw new InvalidCredentialsException("Credenciales inválidas");

            if (user.State != UserStateEnum.ACTIVE)
                throw new UserInactiveException("La cuenta está desactivada");

            return await CreateTokenPairAsync(user, cancellationToken);
        }

        public async Task<LoginResponse> RefreshAsync(RefreshCommand command, CancellationToken cancellationToken = default)
        {
            var tokenHash = tokenService.HashRefreshToken(command.RefreshToken);
            var storedToken = await refreshTokenRepository.GetByHashAsync(tokenHash, cancellationToken);

            if (storedToken is null)
                throw new InvalidCredentialsException("Sesión inválida");

            if (storedToken.RevokedAt is not null)
            {
                // Detección de reuso: un token ya rotado no debería volver a usarse.
                // Se revoca toda la familia del usuario para invalidar un posible token robado.
                await refreshTokenRepository.RevokeAllActiveForUserAsync(storedToken.UserId, cancellationToken);
                await unitOfWork.SaveChangeAsync(cancellationToken);
                throw new InvalidCredentialsException("Sesión inválida");
            }

            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                throw new InvalidCredentialsException("La sesión expiró");

            var user = await userRepository.GetByIdAsync(storedToken.UserId, cancellationToken)
                ?? throw new InvalidCredentialsException("Sesión inválida");

            if (user.State != UserStateEnum.ACTIVE)
                throw new UserInactiveException("La cuenta está desactivada");

            storedToken.RevokedAt = DateTime.UtcNow;

            return await CreateTokenPairAsync(user, cancellationToken);
        }

        public async Task LogoutAsync(LogoutCommand command, CancellationToken cancellationToken = default)
        {
            var tokenHash = tokenService.HashRefreshToken(command.RefreshToken);
            var storedToken = await refreshTokenRepository.GetByHashAsync(tokenHash, cancellationToken);

            if (storedToken is not null && storedToken.RevokedAt is null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                await unitOfWork.SaveChangeAsync(cancellationToken);
            }
        }

        private async Task<LoginResponse> CreateTokenPairAsync(User user, CancellationToken cancellationToken)
        {
            var accessExpiration = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
            var accessToken = tokenService.GenerateToken(user.Id, user.Email, user.Role.ToString(), accessExpiration);

            var refreshTokenValue = tokenService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                TokenHash = tokenService.HashRefreshToken(refreshTokenValue),
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays)
            };

            await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return new LoginResponse
            {
                Token = accessToken,
                RefreshToken = refreshTokenValue,
                Email = user.Email,
                Role = user.Role.ToString(),
                ExpiresAt = accessExpiration,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };
        }

        private static bool IsUniqueViolation(DbUpdateException ex) =>
            ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
    }
}
