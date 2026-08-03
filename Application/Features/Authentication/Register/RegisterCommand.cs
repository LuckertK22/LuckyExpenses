using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Register
{
    public record RegisterCommand(RegisterRequest Request) : IRequest;

    public class RegisterCommandHandler(IAuthenticationService authService) : IRequestHandler<RegisterCommand>
    {
        public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            await authService.RegisterAsync(command.Request, cancellationToken);
        }
    }
}