using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Logout
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; } = null!;
    }
}
