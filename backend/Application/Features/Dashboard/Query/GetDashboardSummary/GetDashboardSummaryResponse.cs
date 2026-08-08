namespace LuckyExpenses.Application.Features.Dashboard.Query.GetDashboardSummary
{
    public class GetDashboardSummaryResponse
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public decimal TotalAmount { get; set; }

        public int TotalExpenses { get; set; }

        public decimal AverageAmount { get; set; }

        public decimal PreviousTotalAmount { get; set; }

        public decimal ChangePercent { get; set; }

        public List<CategoryBreakdownItem> ByCategory { get; set; } = [];
    }

    public class CategoryBreakdownItem
    {
        public long CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public decimal Amount { get; set; }

        public decimal Percentage { get; set; }
    }
}
