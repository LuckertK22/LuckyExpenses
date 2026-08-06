using FluentValidation;

namespace LuckyExpenses.Application.Features.Authentication.Command.Refresh
{
    public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
    {
        public RefreshCommandValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
