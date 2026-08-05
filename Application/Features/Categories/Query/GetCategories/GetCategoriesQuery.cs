using LuckyExpenses.Application.Common;
using MediatR;

namespace LuckyExpenses.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesQuery : IRequest<PagedResponse<GetCategoriesResponse>>
    {
        public string? Search { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;
    }
}
