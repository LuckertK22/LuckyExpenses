using LuckyExpenses.Application.Common;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenses
{
    public class GetExpensesQuery : IRequest<PagedResponse<GetExpensesResponse>>
    {
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public long? CategoryId { get; set; }

        public long? PaymentMethodId { get; set; }

        public string? Title { get; set; }

        public int Page { get; set; } = 1;

        public int Size { get; set; } = 10;
    }
}
