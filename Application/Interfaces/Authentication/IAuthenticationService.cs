using LuckyExpenses.Application.Features.Authentication.Command.Login;
using LuckyExpenses.Application.Features.Authentication.Command.Register;

namespace LuckyExpenses.Application.Interfaces.Authentication
{
    public interface IAuthenticationService
    {
        Task RegisterAsync(RegisterCommand command, CancellationToken cancellationToken = default);
        Task<LoginResponse> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);
    }
}
