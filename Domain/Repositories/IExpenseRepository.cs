using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IExpenseRepository
    {
        Task<Expense[]?> GetByUserAsync(long userId, CancellationToken cancellationToken = default);
        Task<Expense?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(Expense expense, CancellationToken cancellationToken = default);
        void Update(Expense expense);
        void Remove(Expense expense);
    }
}