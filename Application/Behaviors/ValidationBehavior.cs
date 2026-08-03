using FluentValidation;
using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Shared.Utils;
using MediatR;

namespace LuckyExpenses.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var results = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );
                var failures = results.SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(
                        failureGroup => failureGroup.Key.ToLowerEachProperty(),
                        failureGroup => failureGroup.ToArray());

                if (failures.Count != 0)
                {
                    throw new CustomValidationException(failures);
                }
            }

            return await next();
        }
    }
}
