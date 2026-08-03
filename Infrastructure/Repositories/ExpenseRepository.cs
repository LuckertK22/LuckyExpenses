using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class ExpenseRepository(AppDbContext context) : IExpenseRepository
    {
        public async Task<bool> AddAsync(Expense expense, CancellationToken cancellationToken = default)
        {
            await context.Expenses.AddAsync(expense, cancellationToken);
            return true;
        }
    }
}
