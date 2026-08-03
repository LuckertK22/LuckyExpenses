using LuckyExpenses.Domain.Common;

namespace LuckyExpenses.Domain.Entities
{
    public class Expense : BaseEntity
    {
        public long UserId { get; set; }

        public long CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }
    }
}
