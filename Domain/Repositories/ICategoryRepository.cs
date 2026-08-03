using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category[]?> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Category?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(Category category, CancellationToken cancellationToken = default);
    }
}