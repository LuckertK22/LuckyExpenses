using FluentValidation;

namespace LuckyExpenses.Application.Features.Expenses.Command.DeleteExpense
{
    public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
    {
        public DeleteExpenseCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
