using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IPaymentMethodRepository
    {
        Task<PaymentMethod[]?> GetByUserAsync(long userId, CancellationToken cancellationToken = default);
        Task<PaymentMethod?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<bool> AddAsync(PaymentMethod paymentMethod, CancellationToken cancellationToken = default);
        void Remove(PaymentMethod paymentMethod);
    }
}