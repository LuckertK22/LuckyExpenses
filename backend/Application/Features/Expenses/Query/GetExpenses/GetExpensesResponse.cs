namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenses
{
    public class GetExpensesResponse
    {
        public long Id { get; set; }

        public long CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public long? PaymentMethodId { get; set; }

        public string? PaymentMethodName { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
