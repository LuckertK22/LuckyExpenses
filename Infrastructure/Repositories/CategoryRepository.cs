using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<Category[]?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Categories.ToArrayAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.Categories.FindAsync([id], cancellationToken);
        }

        public async Task<bool> AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await context.Categories.AddAsync(category, cancellationToken);
            return true;
        }
    }
}
