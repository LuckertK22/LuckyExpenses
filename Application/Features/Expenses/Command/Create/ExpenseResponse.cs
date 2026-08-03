namespace LuckyExpenses.Application.Features.Expenses.Command.Create
{
    public sealed record ExpenseResponse(
        long Id,
        long CategoryId,
        long? PaymentMethodId,
        string Title,
        string? Description,
        decimal Amount,
        DateTime ExpenseDate,
        DateTime CreatedAt);
}
