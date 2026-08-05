namespace LuckyExpenses.Application.Features.PaymentMethods.Query.GetPaymentMethods
{
    public class GetPaymentMethodsResponse
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
