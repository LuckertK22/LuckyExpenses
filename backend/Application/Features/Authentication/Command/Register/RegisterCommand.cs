using MediatR;

namespace LuckyExpenses.Application.Features.Authentication.Command.Register
{
    public class RegisterCommand : IRequest
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
