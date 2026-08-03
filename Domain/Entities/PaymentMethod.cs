using LuckyExpenses.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyExpenses.Domain.Entities
{
    public class PaymentMethod : BaseEntity
    {
        [Column("user_id")]
        public long UserId { get; set; }

        [Column("name")]
        public string Name { get; set; } = null!;
    }
}