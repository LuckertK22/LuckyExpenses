using LuckyExpenses.Application.Common;
using MediatR;

namespace LuckyExpenses.Application.Features.PaymentMethods.Query.GetPaymentMethods
{
    public class GetPaymentMethodsQuery : IRequest<PagedResponse<GetPaymentMethodsResponse>>
    {
        public string? Search { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;
    }
}
