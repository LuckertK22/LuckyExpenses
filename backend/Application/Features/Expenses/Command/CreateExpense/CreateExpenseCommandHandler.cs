using LuckyExpenses.Application.Context;
using LuckyExpenses.Application.Mappers;
using LuckyExpenses.Domain.Entities;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Domain.Repositories;
using MediatR;

namespace LuckyExpenses.Application.Features.Expenses.Command.CreateExpense
{
    public class CreateExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
        : IRequestHandler<CreateExpenseCommand, CreateExpenseResponse>
    {
        public async Task<CreateExpenseResponse> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new UnauthorizedAccessException("Usuario no autenticado");

            await ValidateReferencesAsync(command, cancellationToken);

            var expense = new Expense
            {
                UserId = currentUser.UserId.Value,
                CategoryId = command.CategoryId,
                PaymentMethodId = command.PaymentMethodId,
                Title = command.Title,
                Description = command.Description,
                Amount = command.Amount,
                ExpenseDate = command.ExpenseDate
            };

            await expenseRepository.AddAsync(expense, cancellationToken);
            await unitOfWork.SaveChangeAsync(cancellationToken);

            return ExpenseMapper.ToResponse(expense);
        }

        private async Task ValidateReferencesAsync(CreateExpenseCommand command, CancellationToken cancellationToken)
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
