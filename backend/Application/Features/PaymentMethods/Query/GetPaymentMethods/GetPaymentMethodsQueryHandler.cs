using LuckyExpenses.Application.Common;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.PaymentMethods.Query.GetPaymentMethods
{
    public class GetPaymentMethodsQueryHandler(IPaymentMethodRepository paymentMethodRepository)
        : IRequestHandler<GetPaymentMethodsQuery, PagedResponse<GetPaymentMethodsResponse>>
    {
        public async Task<PagedResponse<GetPaymentMethodsResponse>> Handle(GetPaymentMethodsQuery query, CancellationToken cancellationToken)
        {
            var (totalCount, items) = await paymentMethodRepository.SearchAsync(
                query.Search,
                query.Page,
                query.Size,
                cancellationToken);

            return new PagedResponse<GetPaymentMethodsResponse>
            {
                Items = items.Select(PaymentMethodMapper.ToListItem).ToArray(),
                TotalItems = totalCount,
                Page = query.Page,
                Size = query.Size
            };
        }
    }
}
