using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity, TKey>(AppDbContext context) : IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : notnull
    {
        protected DbSet<TEntity> DbSet => context.Set<TEntity>();

        public async Task<TEntity[]?> GetAll(CancellationToken cancellationToken = default)
            => await DbSet.ToArrayAsync(cancellationToken);

        public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
            => await DbSet.FindAsync([id], cancellationToken);

        public async Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
            return true;
        }

        public void Remove(TEntity entity)
            => DbSet.Remove(entity);

        public async Task<(int TotalCount, TEntity[] Items)> GetPagedAsync(
            IQueryable<TEntity> query,
            int page,
            int size,
            CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToArrayAsync(cancellationToken);

            return (totalCount, items);
        }
    }
}
