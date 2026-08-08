using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class PaymentMethodRepository(AppDbContext context) : BaseRepository<PaymentMethod, long>(context), IPaymentMethodRepository
    {
        public async Task<(int TotalCount, PaymentMethod[] Items)> SearchAsync(
            string? search,
            int page,
            int size,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p =>
                    EF.Functions.ILike(EF.Functions.Unaccent(p.Name), EF.Functions.Unaccent($"%{search}%")) ||
                    EF.Functions.ILike(EF.Functions.Unaccent(p.Code), EF.Functions.Unaccent($"%{search}%")));

            query = query.OrderBy(p => p.Name);

            return await GetPagedAsync(query, page, size, cancellationToken);
        }
    }
}
