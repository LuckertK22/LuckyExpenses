using LuckyExpenses.Application.Features.Authentication.Common;
using LuckyExpenses.Application.Features.Authentication.Login;
using LuckyExpenses.Application.Features.Authentication.Register;
using LuckyExpenses.Application.Features.Users.Common;
using LuckyExpenses.Application.Interfaces.Authentication;
using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Enums;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace LuckyExpenses.Infrastructure.Authentication
{
    public class AuthenticationService(
        IUserRepository userRepository,
        IHasherService hasherService,
        ITokenService tokenService,
        IUnitOfWork unitOfWork) : IAuthenticationService
    {
        private const int TokenDurationHours = 8;

        public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (exists is not null)
                throw new ConflictException("El correo electrónico ya está registrado");

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hasherService.Hash(request.Password),
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

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken)
                ?? throw new InvalidCredentialsException("Credenciales inválidas");

            if (!hasherService.Verify(request.Password, user.PasswordHash))
                throw new InvalidCredentialsException("Credenciales inválidas");

            if (user.State != UserStateEnum.ACTIVE)
                throw new UserInactiveException("La cuenta está desactivada");

            var expiration = DateTime.UtcNow.AddHours(TokenDurationHours);
            var token = tokenService.GenerateToken(user.Id, user.Email, user.Role.ToString(), expiration);

            return new AuthenticationResponse(token, user.Email, user.Role.ToString(), expiration);
        }

        private static bool IsUniqueViolation(DbUpdateException ex) =>
            ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };

        public async Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetByIdAsync(userId, cancellationToken)
                ?? throw new NotFoundException("Usuario no encontrado");

            return new UserResponse(user.Id, user.FirstName, user.LastName, user.Email, user.Role.ToString());
        }
    }
}