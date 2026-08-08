using LuckyExpenses.Application.Context;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Command.UpdateExpense
{
    public class UpdateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
        : IRequestHandler<UpdateExpenseCommand, UpdateExpenseResponse>
    {
        public async Task<UpdateExpenseResponse> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            await ValidateReferencesAsync(command, cancellationToken);

            var expense = await expenseRepository.GetByIdForUserAsync(command.Id, currentUser.UserId.Value, cancellationToken)
                ?? throw new NotFoundException("El gasto especificado no existe");

            expense.CategoryId = command.CategoryId;
            expense.PaymentMethodId = command.PaymentMethodId;
            expense.Title = command.Title;
            expense.Description = command.Description;
            expense.Amount = command.Amount;
            expense.ExpenseDate = command.ExpenseDate;

            await unitOfWork.SaveChangeAsync(cancellationToken);

            return ExpenseMapper.ToUpdateResponse(expense);
        }

        private async Task ValidateReferencesAsync(UpdateExpenseCommand command, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
            if (category is null)
                throw new NotFoundException("La categoría especificada no existe");

            if (command.PaymentMethodId.HasValue)
            {
                var paymentMethod = await paymentMethodRepository.GetByIdAsync(command.PaymentMethodId.Value, cancellationToken);
                if (paymentMethod is null)
                    throw new NotFoundException("El método de pago especificado no existe");
            }
        }
    }
}
