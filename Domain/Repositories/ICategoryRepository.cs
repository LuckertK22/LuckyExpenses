using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category, long>
    {
        Task<(int TotalCount, Category[] Items)> SearchAsync(
            string? search,
            int page,
            int size,
            CancellationToken cancellationToken = default);
    }
}
