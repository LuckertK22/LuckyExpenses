namespace LuckyExpenses.Domain.Repositories
{
    public interface IBaseRepository<TEntity, TKey> where TEntity : class where TKey : notnull
    {
        Task<TEntity[]?> GetAll(CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
    }
}