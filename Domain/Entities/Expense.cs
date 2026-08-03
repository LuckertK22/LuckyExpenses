using LuckyExpenses.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyExpenses.Domain.Entities
{
    public class Expense : BaseEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("category_id")]
        public long CategoryId { get; set; }

        [Column("payment_method_id")]
        public long? PaymentMethodId { get; set; }

        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("expense_date")]
        public DateTime ExpenseDate { get; set; }
    }
}