using FluentValidation;

namespace LuckyExpenses.Application.Features.Expenses.Query.GetExpenses
{
    public class GetExpensesQueryValidator : AbstractValidator<GetExpensesQuery>
    {
        public GetExpensesQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("La página debe ser mayor a cero");

            RuleFor(x => x.Size)
                .GreaterThan(0)
                .WithMessage("El tamaño de página debe ser mayor a cero")
                .LessThanOrEqualTo(100)
                .WithMessage("El tamaño de página no puede exceder 100");

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("La fecha final no puede ser anterior a la fecha inicial");
        }
    }
}
