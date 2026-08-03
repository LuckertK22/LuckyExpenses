using LuckyExpenses.Infrastructure.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace LuckyExpenses.WebAPI.Middlewares
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ExceptionExtension.HandleException(exception, out var response);

            httpContext.Response.StatusCode = (int)response.HttpStatusCode;
            httpContext.Response.ContentType = "application/json";

            string jsonResponse = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(jsonResponse, cancellationToken);

            return Task.CompletedTask.IsCompletedSuccessfully;
        }
    }
}