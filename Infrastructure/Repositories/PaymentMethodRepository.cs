using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Repositories;
using LuckyExpenses.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LuckyExpenses.Infrastructure.Repositories
{
    public class PaymentMethodRepository(AppDbContext context) : IPaymentMethodRepository
    {
        public async Task<PaymentMethod[]?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.PaymentMethods.ToArrayAsync(cancellationToken);
        }

        public async Task<PaymentMethod?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await context.PaymentMethods.FindAsync([id], cancellationToken);
        }

        public async Task<bool> AddAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken = default)
        {
            await context.PaymentMethods.AddAsync(paymentMethod, cancellationToken);
            return true;
        }

        public void Remove(PaymentMethod paymentMethod)
        {
            context.PaymentMethods.Remove(paymentMethod);
        }
    }
}
