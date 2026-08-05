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
    }
}
