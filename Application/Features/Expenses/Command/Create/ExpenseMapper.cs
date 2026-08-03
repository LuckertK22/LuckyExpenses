using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Application.Features.Expenses.Command.Create
{
    internal static class ExpenseMapper
    {
        public static ExpenseResponse ToResponse(Expense expense) =>
            new(
                expense.Id,
                expense.CategoryId,
                expense.PaymentMethodId,
                expense.Title,
                expense.Description,
                expense.Amount,
                expense.ExpenseDate,
                expense.CreatedAt);
    }
}
