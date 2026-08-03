using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class ExpenseRepository(AppDbContext context) : IExpenseRepository
    {
        public async Task<Expense[]?> GetByUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await context.Expenses
                .Where(e => e.UserId == userId)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<Expense?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.Expenses.FindAsync([id], cancellationToken);
        }

        public async Task<bool> AddAsync(Expense expense, CancellationToken cancellationToken = default)
        {
            await context.Expenses.AddAsync(expense, cancellationToken);
            return true;
        }

        public void Update(Expense expense)
        {
            context.Expenses.Update(expense);
        }

        public void Remove(Expense expense)
        {
            context.Expenses.Remove(expense);
        }
    }
}
