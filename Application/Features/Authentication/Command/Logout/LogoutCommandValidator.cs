using FluentValidation;

namespace LuckyExpenses.Application.Features.Authentication.Command.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
