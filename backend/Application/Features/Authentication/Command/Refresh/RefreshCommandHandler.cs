using LuckyExpenses.Application.Features.Authentication.Command.Login;
using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Refresh
{
    public class RefreshCommandHandler(IAuthenticationService authService)
        : IRequestHandler<RefreshCommand, LoginResponse>
    {
        public Task<LoginResponse> Handle(RefreshCommand command, CancellationToken cancellationToken)
        {
            return authService.RefreshAsync(command, cancellationToken);
        }
    }
}
