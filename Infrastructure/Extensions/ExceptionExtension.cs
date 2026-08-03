using LuckyExpenses.Domain.Exceptions;
using LuckyExpenses.Shared.Common;
using System.Net;

namespace LuckyExpenses.Infrastructure.Extensions
{
    public static class ExceptionExtension
    {
        public static void HandleException(Exception exception, out ApiResponse<object> response)
        {
            response = new()
            {
                Ok = false,
                Message = "Error interno en el servidor",
                Data = null,
                Errors = exception.Message,
                HttpStatusCode = HttpStatusCode.InternalServerError
            };

            switch (exception)
            {
                case CustomValidationException _ex:
                    response.Message = _ex.Message;
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    response.Errors = _ex.Errors;
                    break;
                case NotFoundException _ex:
                    response.Message = _ex.Message;
                    response.HttpStatusCode = HttpStatusCode.NotFound;
                    break;
                case InvalidCredentialsException _ex:
                    response.Message = _ex.Message;
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedAccessException _ex:
                    response.Message = _ex.Message;
                    response.HttpStatusCode = HttpStatusCode.Unauthorized;
                    break;
                case InvalidOperationException _ex:
                    response.Message = _ex.Message;
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    break;
            }
        }
    }
}
