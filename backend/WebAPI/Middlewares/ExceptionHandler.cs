using LuckyExpenses.WebAPI.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LuckyExpenses.WebAPI.Middlewares
{
    public class ExceptionHandler(ILogger<ExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = exception.ToProblemDetails();

            if (problemDetails.Status is StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Excepción no controlada: {Message}", exception.Message);
            else
                logger.LogWarning(exception, "Excepción controlada: {Message}", exception.Message);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }
    }
}
