namespace LuckyExpenses.Application.Features.Expenses.Command.CreateExpense
{
    public class CreateExpenseResponse
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public long? PaymentMethodId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
