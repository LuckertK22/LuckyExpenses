using LuckyExpenses.Domain.Common;

namespace LuckyExpenses.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public long UserId { get; set; }

        public string TokenHash { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
