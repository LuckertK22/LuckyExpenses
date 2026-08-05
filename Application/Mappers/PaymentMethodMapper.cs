using LuckyExpenses.Application.Features.PaymentMethods.Query.GetPaymentMethods;
using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Application.Mappers
{
    internal static class PaymentMethodMapper
    {
        public static GetPaymentMethodsResponse ToListItem(PaymentMethod paymentMethod) =>
            new()
            {
                Id = paymentMethod.Id,
                Code = paymentMethod.Code,
                Name = paymentMethod.Name,
                CreatedAt = paymentMethod.CreatedAt
            };
    }
}
