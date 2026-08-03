using LuckyExpenses.Application.Features.Users.Common;
using LuckyExpenses.Application.Interfaces.Authentication;
using MediatR;

namespace LuckyExpenses.Application.Features.Users.GetCurrentUser
{
    public record GetCurrentUserQuery(long UserId) : IRequest<UserResponse>;

    public class GetCurrentUserQueryHandler(IAuthenticationService authService)
        : IRequestHandler<GetCurrentUserQuery, UserResponse>
    {
        public Task<UserResponse> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            return authService.GetUserAsync(query.UserId, cancellationToken);
        }
    }
}
