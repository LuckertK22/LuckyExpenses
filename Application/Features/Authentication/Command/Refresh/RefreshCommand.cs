using LuckyExpenses.Application.Features.Authentication.Command.Login;
using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Refresh
{
    public class RefreshCommand : IRequest<LoginResponse>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
