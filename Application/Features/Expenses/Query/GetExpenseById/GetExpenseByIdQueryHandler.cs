using LuckyExpenses.Application.Context;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenseById
{
    public class GetExpenseByIdQueryHandler(
        IExpenseRepository expenseRepository,
        ICurrentUser currentUser)
        : IRequestHandler<GetExpenseByIdQuery, GetExpenseByIdResponse>
    {
        public async Task<GetExpenseByIdResponse> Handle(GetExpenseByIdQuery query, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var expense = await expenseRepository.GetByIdForUserAsync(query.Id, currentUser.UserId.Value, cancellationToken)
                ?? throw new NotFoundException("El gasto especificado no existe");

            return ExpenseMapper.ToByIdResponse(expense);
        }
    }
}
