using FluentValidation;

namespace LuckyExpenses.Application.Features.Dashboard.Query.GetDashboardSummary
{
    public class GetDashboardSummaryQueryValidator : AbstractValidator<GetDashboardSummaryQuery>
    {
        public GetDashboardSummaryQueryValidator()
        {
            RuleFor(x => x.Month)
                .InclusiveBetween(1, 12)
                .WithMessage("El mes debe estar entre 1 y 12")
                .When(x => x.Month.HasValue);

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, 2100)
                .WithMessage("El año debe estar entre 1900 y 2100")
                .When(x => x.Year.HasValue);
        }
    }
}
