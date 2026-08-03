using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Application.Context
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        long? UserId { get; }
        string? Email { get; }
        string? Role { get; }

        Task<User?> GetUserAsync(CancellationToken cancellationToken = default);
    }
}
