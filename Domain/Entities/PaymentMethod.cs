using LuckyExpenses.Domain.Common;

namespace LuckyExpenses.Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public long UserId { get; set; }

        public string Name { get; set; } = null!;
    }
}
