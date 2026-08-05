using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Domain.Repositories
{
    public interface IPaymentMethodRepository : IBaseRepository<PaymentMethod, long>
    {
        Task<(int TotalCount, PaymentMethod[] Items)> SearchAsync(
            string? search,
            int page,
            int size,
            CancellationToken cancellationToken = default);
    }
}
