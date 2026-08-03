namespace LuckyExpenses.Domain.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> SaveChangeAsync(CancellationToken? cancellationToken);
        Task BeginTransactionAsync(CancellationToken? cancellationToken = null);
        Task CommitTransactionAsync(CancellationToken? cancellationToken = null);
        Task RollbackTransactionAsync(CancellationToken? cancellationToken = null);
        bool IsInTransaction { get; }
    }
}