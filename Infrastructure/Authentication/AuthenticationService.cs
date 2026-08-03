using LuckyExpenses.Application.Features.Authentication.Common;
using LuckyExpenses.Application.Features.Authentication.Login;
using LuckyExpenses.Application.Features.Authentication.Register;
using LuckyExpenses.Application.Features.Users.Common;
using LuckyExpenses.Application.Interfaces.Authentication;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Services;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace LuckyExpenses.Infrastructure.Authentication
{
    public class AuthenticationService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<long>> roleManager,
        ITokenService tokenService) : IAuthenticationService
    {
        private const string DefaultRole = "USER";

        public async Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var exists = await userManager.FindByEmailAsync(request.Email);
            if (exists is not null)
                throw new InvalidCredentialsException("El correo electrónico ya está registrado");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new DomainException(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await roleManager.RoleExistsAsync(DefaultRole))
                await roleManager.CreateAsync(new IdentityRole<long> { Name = DefaultRole });

            await userManager.AddToRoleAsync(user, DefaultRole);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                throw new InvalidCredentialsException("Credenciales inválidas");

            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? DefaultRole;

            var expiration = DateTime.UtcNow.AddHours(8);
            var token = tokenService.GenerateToken(user.Id, user.Email!, role);

            return new AuthenticationResponse(token, user.Email!, role, expiration);
        }

        public async Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new NotFoundException("Usuario no encontrado");

            var role = (await userManager.GetRolesAsync(user)).FirstOrDefault() ?? DefaultRole;

            return new UserResponse(user.Id, user.FirstName, user.LastName, user.Email!, role);
        }
    }
}