using LuckyExpenses.Application.Features.Expenses.Command.CreateExpense;
using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Application.Mappers
{
    internal static class ExpenseMapper
    {
        public static CreateExpenseResponse ToResponse(Expense expense) =>
            new()
            {
                Id = expense.Id,
                CategoryId = expense.CategoryId,
                PaymentMethodId = expense.PaymentMethodId,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                CreatedAt = expense.CreatedAt
            };
    }
}
