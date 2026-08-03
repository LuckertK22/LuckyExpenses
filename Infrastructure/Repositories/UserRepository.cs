using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.Users.FindAsync([id], cancellationToken);
        }

        public async Task<bool> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await context.Users.AddAsync(user, cancellationToken);
            return true;
        }
    }
}