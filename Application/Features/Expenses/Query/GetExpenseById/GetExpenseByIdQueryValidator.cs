using FluentValidation;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenseById
{
    public class GetExpenseByIdQueryValidator : AbstractValidator<GetExpenseByIdQuery>
    {
        public GetExpenseByIdQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
