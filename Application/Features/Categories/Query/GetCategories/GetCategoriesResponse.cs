namespace LuckyExpenses.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesResponse
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
