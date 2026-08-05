using LuckyExpenses.Application.Features.Authentication.Command.Login;
using LuckyExpenses.Application.Features.Authentication.Command.Register;
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

            var expiration = DateTime.UtcNow.AddHours(TokenDurationHours);
            var token = tokenService.GenerateToken(user.Id, user.Email, user.Role.ToString(), expiration);

            return new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.ToString(),
                ExpiresAt = expiration
            };
        }

        private static bool IsUniqueViolation(DbUpdateException ex) =>
            ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };
    }
}