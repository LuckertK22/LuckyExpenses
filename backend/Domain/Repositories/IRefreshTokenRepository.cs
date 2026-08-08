using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, long>
    {
        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default);

        Task RevokeAllActiveForUserAsync(long userId, CancellationToken cancellationToken = default);
    }
}
