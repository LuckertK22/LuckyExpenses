using LuckyExpenses.Application.Context;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Dashboard.Query.GetDashboardSummary
{
    public class GetDashboardSummaryQueryHandler(
        IExpenseRepository expenseRepository,
        ICurrentUser currentUser)
        : IRequestHandler<GetDashboardSummaryQuery, GetDashboardSummaryResponse>
    {
        public async Task<GetDashboardSummaryResponse> Handle(GetDashboardSummaryQuery query, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var year = query.Year ?? DateTime.UtcNow.Year;
            var month = query.Month ?? DateTime.UtcNow.Month;

            var periodStart = new DateTime(year, month, 1);
            var periodEnd = periodStart.AddMonths(1).AddTicks(-1);
            var previousPeriodStart = periodStart.AddMonths(-1);
            var previousPeriodEnd = periodStart.AddTicks(-1);

            var summary = await expenseRepository.GetDashboardSummaryAsync(
                currentUser.UserId.Value,
                periodStart,
                periodEnd,
                previousPeriodStart,
                previousPeriodEnd,
                cancellationToken);

            var totalAmount = summary.TotalAmount;
            var changePercent = summary.PreviousTotalAmount > 0
                ? (totalAmount - summary.PreviousTotalAmount) / summary.PreviousTotalAmount * 100
                : totalAmount > 0
                    ? 100
                    : 0;

            var byCategory = summary.ByCategory
                .Select(item => new CategoryBreakdownItem
                {
                    CategoryId = item.CategoryId,
                    CategoryName = item.CategoryName,
                    Amount = item.Amount,
                    Percentage = totalAmount > 0
                        ? Math.Round(item.Amount / totalAmount * 100, 2)
                        : 0
                })
                .ToArray();

            return new GetDashboardSummaryResponse
            {
                Year = year,
                Month = month,
                TotalAmount = totalAmount,
                TotalExpenses = summary.TotalCount,
                AverageAmount = summary.TotalCount > 0
                    ? Math.Round(totalAmount / summary.TotalCount, 2)
                    : 0,
                PreviousTotalAmount = summary.PreviousTotalAmount,
                ChangePercent = Math.Round(changePercent, 2),
                ByCategory = [.. byCategory]
            };
        }
    }
}
