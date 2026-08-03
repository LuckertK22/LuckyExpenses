using LuckyExpenses.Domain.Common;

namespace LuckyExpenses.Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
