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

        Task<ExpenseDashboardSummary> GetDashboardSummaryAsync(
            long userId,
            DateTime periodStart,
            DateTime periodEnd,
            DateTime previousPeriodStart,
            DateTime previousPeriodEnd,
            CancellationToken cancellationToken = default);
    }

    public record ExpenseDashboardSummary(
        decimal TotalAmount,
        int TotalCount,
        decimal PreviousTotalAmount,
        ExpenseCategorySummary[] ByCategory);

    public record ExpenseCategorySummary(
        long CategoryId,
        string CategoryName,
        decimal Amount);
}
