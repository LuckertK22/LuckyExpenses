using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class ExpenseRepository(AppDbContext context) : BaseRepository<Expense, long>(context), IExpenseRepository
    {
        public async Task<(int TotalCount, Expense[] Items)> GetByUserAsync(
            long userId,
            DateTime? fromDate,
            DateTime? toDate,
            long? categoryId,
            long? paymentMethodId,
            string? title,
            int page,
            int size,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet
                .Include(e => e.Category)
                .Include(e => e.PaymentMethod)
                .Where(e => e.UserId == userId);

            if (fromDate.HasValue)
                query = query.Where(e => e.ExpenseDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(e => e.ExpenseDate <= toDate.Value);

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            if (paymentMethodId.HasValue)
                query = query.Where(e => e.PaymentMethodId == paymentMethodId.Value);

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => EF.Functions.ILike(
                    EF.Functions.Unaccent(e.Title),
                    EF.Functions.Unaccent($"%{title}%")));

            query = query.OrderByDescending(e => e.ExpenseDate).ThenByDescending(e => e.Id);

            return await GetPagedAsync(query, page, size, cancellationToken);
        }

        public async Task<Expense?> GetByIdForUserAsync(
            long id,
            long userId,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Include(e => e.Category)
                .Include(e => e.PaymentMethod)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId, cancellationToken);
        }

        public async Task<ExpenseDashboardSummary> GetDashboardSummaryAsync(
            long userId,
            DateTime periodStart,
            DateTime periodEnd,
            DateTime previousPeriodStart,
            DateTime previousPeriodEnd,
            CancellationToken cancellationToken = default)
        {
            var userQuery = DbSet.Where(e => e.UserId == userId);

            var periodQuery = userQuery.Where(e => e.ExpenseDate >= periodStart && e.ExpenseDate <= periodEnd);
            var previousPeriodQuery = userQuery.Where(e => e.ExpenseDate >= previousPeriodStart && e.ExpenseDate <= previousPeriodEnd);

            var totalAmount = await periodQuery.SumAsync(e => e.Amount, cancellationToken);
            var totalCount = await periodQuery.CountAsync(cancellationToken);
            var previousTotalAmount = await previousPeriodQuery.SumAsync(e => e.Amount, cancellationToken);

            var byCategory = await periodQuery
                .Include(e => e.Category)
                .GroupBy(e => new { e.CategoryId, CategoryName = e.Category.Name })
                .Select(g => new ExpenseCategorySummary(
                    g.Key.CategoryId,
                    g.Key.CategoryName,
                    g.Sum(e => e.Amount)))
                .OrderByDescending(g => g.Amount)
                .ToArrayAsync(cancellationToken);

            return new ExpenseDashboardSummary(totalAmount, totalCount, previousTotalAmount, byCategory);
        }
    }
}
