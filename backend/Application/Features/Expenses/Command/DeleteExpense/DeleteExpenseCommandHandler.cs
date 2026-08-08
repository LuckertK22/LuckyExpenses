using LuckyExpenses.Application.Context;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Command.DeleteExpense
{
    public class DeleteExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
        : IRequestHandler<DeleteExpenseCommand>
    {
        public async Task Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            var expense = await expenseRepository.GetByIdForUserAsync(command.Id, currentUser.UserId.Value, cancellationToken)
                ?? throw new NotFoundException("El gasto especificado no existe");

            expenseRepository.Remove(expense);
            await unitOfWork.SaveChangeAsync(cancellationToken);
        }
    }
}
