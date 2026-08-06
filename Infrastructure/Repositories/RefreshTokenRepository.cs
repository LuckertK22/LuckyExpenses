using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class RefreshTokenRepository(AppDbContext context) : BaseRepository<RefreshToken, long>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(t => t.TokenHash == tokenHash, cancellationToken);
        }

        public async Task RevokeAllActiveForUserAsync(long userId, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var activeTokens = await DbSet
                .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > now)
                .ToArrayAsync(cancellationToken);

            foreach (var token in activeTokens)
                token.RevokedAt = now;
        }
    }
}
