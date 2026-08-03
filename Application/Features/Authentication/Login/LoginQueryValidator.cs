using FluentValidation;
using LuckyExpenses.Application.Features.Authentication.Login;

namespace LuckyExpenses.Application.Features.Authentication.Login
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Request.Password).NotEmpty();
        }
    }
}