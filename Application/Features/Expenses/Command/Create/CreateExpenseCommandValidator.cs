using FluentValidation;

namespace LuckyExpenses.Application.Features.Expenses.Command.Create
{
    public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        public CreateExpenseCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(500);
            RuleFor(x => x.Amount).GreaterThan(0m).LessThanOrEqualTo(999999999999.99m);
            RuleFor(x => x.ExpenseDate).NotEmpty();
            RuleFor(x => x.CategoryId).GreaterThan(0);
        }
    }
}
