using LuckyExpenses.Application.Common;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
        : IRequestHandler<GetCategoriesQuery, PagedResponse<GetCategoriesResponse>>
    {
        public async Task<PagedResponse<GetCategoriesResponse>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
        {
            var (totalCount, items) = await categoryRepository.SearchAsync(
                query.Search,
                query.Page,
                query.Size,
                cancellationToken);

            return new PagedResponse<GetCategoriesResponse>
            {
                Items = items.Select(CategoryMapper.ToListItem).ToArray(),
                TotalItems = totalCount,
                Page = query.Page,
                Size = query.Size
            };
        }
    }
}
