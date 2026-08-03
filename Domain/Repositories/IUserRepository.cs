using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
    }
}