using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
