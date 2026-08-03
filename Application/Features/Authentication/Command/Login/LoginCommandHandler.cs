using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Login
{
    public class LoginCommandHandler(IAuthenticationService authService)
        : IRequestHandler<LoginCommand, LoginResponse>
    {
        public Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            return authService.LoginAsync(command, cancellationToken);
        }
    }
}
