using LuckyExpenses.Domain.Common;

namespace LuckyExpenses.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
