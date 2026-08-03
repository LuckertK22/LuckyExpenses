using LuckyExpenses.Application.Features.Authentication.Common;
using LuckyExpenses.Application.Features.Authentication.Login;
using LuckyExpenses.Application.Features.Authentication.Register;
using LuckyExpenses.Application.Features.Users.Common;

namespace LuckyExpenses.Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<UserResponse> GetUserAsync(long userId, CancellationToken cancellationToken = default);
    }
}