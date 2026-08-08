using MediatR;

namespace LuckyExpenses.Application.Features.Dashboard.Query.GetDashboardSummary
{
    public class GetDashboardSummaryQuery : IRequest<GetDashboardSummaryResponse>
    {
        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}
