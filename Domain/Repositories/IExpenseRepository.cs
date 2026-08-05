using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense, long>
    {
        Task<(int TotalCount, Expense[] Items)> GetByUserAsync(
            long userId,
            DateTime? fromDate,
            DateTime? toDate,
            long? categoryId,
            long? paymentMethodId,
            string? title,
            int page,
            int size,
            CancellationToken cancellationToken = default);

        Task<Expense?> GetByIdForUserAsync(
            long id,
            long userId,
            CancellationToken cancellationToken = default);
    }
}
