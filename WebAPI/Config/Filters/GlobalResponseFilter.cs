using LuckyExpenses.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LuckyExpenses.WebAPI.Config.Filters
{
    public class GlobalResponseFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult && IsSuccessStatus(objectResult.StatusCode ?? StatusCodes.Status200OK))
            {
                if (objectResult.Value is not null && !IsApiResponse(objectResult.Value.GetType()))
                {
                    var dataType = objectResult.DeclaredType ?? objectResult.Value.GetType();
                    if (dataType == typeof(object))
                        dataType = objectResult.Value.GetType();

                    var responseType = typeof(ApiResponse<>).MakeGenericType(dataType);
                    objectResult.Value = Activator.CreateInstance(responseType, "Respuesta exitosa", objectResult.Value);
                    objectResult.DeclaredType = responseType;
                }
            }
            else if (context.Result is StatusCodeResult statusCodeResult && IsSuccessStatus(statusCodeResult.StatusCode))
            {
                context.Result = new ObjectResult(new ApiResponse<object>("Respuesta exitosa", null))
                {
                    StatusCode = statusCodeResult.StatusCode
                };
            }

            await next();
        }

        private static bool IsSuccessStatus(int statusCode) => statusCode >= 200 && statusCode < 300;

        private static bool IsApiResponse(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>);
    }
}
