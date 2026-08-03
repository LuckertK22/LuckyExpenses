using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private bool _isInTransaction;
        public bool IsInTransaction => _isInTransaction;

        public async Task<int> SaveChangeAsync(CancellationToken? cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken ?? CancellationToken.None);
        }

        public async Task BeginTransactionAsync(CancellationToken? cancellationToken = null)
        {
            await context.Database.BeginTransactionAsync(cancellationToken ?? CancellationToken.None);
            _isInTransaction = true;
        }

        public async Task CommitTransactionAsync(CancellationToken? cancellationToken = null)
        {
            await context.Database.CommitTransactionAsync(cancellationToken ?? CancellationToken.None);
            _isInTransaction = false;
        }

        public async Task RollbackTransactionAsync(CancellationToken? cancellationToken = null)
        {
            await context.Database.RollbackTransactionAsync(cancellationToken ?? CancellationToken.None);
            _isInTransaction = false;
        }

        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}