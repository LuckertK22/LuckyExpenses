using FluentValidation;

namespace LuckyExpenses.Application.Features.Categories.Query.GetCategories
{
    public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
    {
        public GetCategoriesQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("La página debe ser mayor a cero");

            RuleFor(x => x.Size)
                .GreaterThan(0)
                .WithMessage("El tamaño de página debe ser mayor a cero")
                .LessThanOrEqualTo(100)
                .WithMessage("El tamaño de página no puede exceder 100");
        }
    }
}
