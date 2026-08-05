using LuckyExpenses.Application.Common;
using LuckyExpenses.Application.Context;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenses
{
    public class GetExpensesQueryHandler(
        IExpenseRepository expenseRepository,
        ICurrentUser currentUser)
        : IRequestHandler<GetExpensesQuery, PagedResponse<GetExpensesResponse>>
    {
        public async Task<PagedResponse<GetExpensesResponse>> Handle(GetExpensesQuery query, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var (totalCount, items) = await expenseRepository.GetByUserAsync(
                currentUser.UserId.Value,
                query.FromDate,
                query.ToDate,
                query.CategoryId,
                query.PaymentMethodId,
                query.Title,
                query.Page,
                query.Size,
                cancellationToken);

            return new PagedResponse<GetExpensesResponse>
            {
                Items = items.Select(ExpenseMapper.ToListItem).ToArray(),
                TotalItems = totalCount,
                Page = query.Page,
                Size = query.Size
            };
        }
    }
}
