using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext context) : BaseRepository<Category, long>(context), ICategoryRepository
    {
        public async Task<(int TotalCount, Category[] Items)> SearchAsync(
            string? search,
            int page,
            int size,
            CancellationToken cancellationToken = default)
        {
            var query = DbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c =>
                    EF.Functions.ILike(EF.Functions.Unaccent(c.Name), EF.Functions.Unaccent($"%{search}%")) ||
                    EF.Functions.ILike(EF.Functions.Unaccent(c.Code), EF.Functions.Unaccent($"%{search}%")));

            query = query.OrderBy(c => c.Name);

            return await GetPagedAsync(query, page, size, cancellationToken);
        }
    }
}
