using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Logout
{
    public class LogoutCommandHandler(IAuthenticationService authService)
        : IRequestHandler<LogoutCommand>
    {
        public Task Handle(LogoutCommand command, CancellationToken cancellationToken)
        {
            return authService.LogoutAsync(command, cancellationToken);
        }
    }
}
