using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Command.DeleteExpense
{
    public class DeleteExpenseCommand : IRequest
    {
        public long Id { get; set; }
    }
}
