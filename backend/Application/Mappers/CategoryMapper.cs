using LuckyExpenses.Application.Features.Categories.Query.GetCategories;
using LuckyExpenses.Domain.Entities;

namespace LuckyExpenses.Application.Mappers
{
    internal static class CategoryMapper
    {
        public static GetCategoriesResponse ToListItem(Category category) =>
            new()
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
    }
}
