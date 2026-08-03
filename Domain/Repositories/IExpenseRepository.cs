using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IExpenseRepository
    {
        Task<bool> AddAsync(Expense expense, CancellationToken cancellationToken = default);
    }
}
