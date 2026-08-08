using LuckyExpenses.Application.Features.Expenses.Command.CreateExpense;
using LuckyExpenses.Application.Features.Expenses.Command.UpdateExpense;
using LuckyExpenses.Application.Features.Expenses.Query.GetExpenseById;
using LuckyExpenses.Application.Features.Expenses.Query.GetExpenses;
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

        public static GetExpensesResponse ToListItem(Expense expense) =>
            new()
            {
                Id = expense.Id,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name ?? string.Empty,
                PaymentMethodId = expense.PaymentMethodId,
                PaymentMethodName = expense.PaymentMethod?.Name,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                CreatedAt = expense.CreatedAt
            };

        public static GetExpenseByIdResponse ToByIdResponse(Expense expense) =>
            new()
            {
                Id = expense.Id,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name ?? string.Empty,
                PaymentMethodId = expense.PaymentMethodId,
                PaymentMethodName = expense.PaymentMethod?.Name,
                Title = expense.Title,
                Description = expense.Description,
                Amount = expense.Amount,
                ExpenseDate = expense.ExpenseDate,
                CreatedAt = expense.CreatedAt
            };

        public static UpdateExpenseResponse ToUpdateResponse(Expense expense) =>
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
