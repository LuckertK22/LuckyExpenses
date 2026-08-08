using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenseById
{
    public class GetExpenseByIdQuery : IRequest<GetExpenseByIdResponse>
    {
        public long Id { get; set; }
    }
}
