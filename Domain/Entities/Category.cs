using LuckyExpenses.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuckyExpenses.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("icon")]
        public string? Icon { get; set; }
    }
}