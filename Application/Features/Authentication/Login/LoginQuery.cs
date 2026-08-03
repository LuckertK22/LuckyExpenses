using LuckyExpenses.Application.Features.Authentication.Common;
using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Login
{
    public record LoginQuery(LoginRequest Request) : IRequest<AuthenticationResponse>;

    public class LoginQueryHandler(IAuthenticationService authService)
        : IRequestHandler<LoginQuery, AuthenticationResponse>
    {
        public Task<AuthenticationResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            return authService.LoginAsync(request.Request, cancellationToken);
        }
    }
}