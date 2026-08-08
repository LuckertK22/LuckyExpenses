using LuckyExpenses.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LuckyExpenses.WebAPI.Extensions
{
    public static class ExceptionExtension
    {
        public static ProblemDetails ToProblemDetails(this Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Error interno en el servidor",
                Detail = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo."
            };

            switch (exception)
            {
                case CustomValidationException validationException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Solicitud inválida";
                    problemDetails.Detail = validationException.Message;
                    problemDetails.Extensions["errors"] = validationException.Errors;
                    break;
                case NotFoundException notFoundException:
                    problemDetails.Status = (int)HttpStatusCode.NotFound;
                    problemDetails.Title = "Recurso no encontrado";
                    problemDetails.Detail = notFoundException.Message;
                    break;
                case InvalidCredentialsException invalidCredentialsException:
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Title = "Credenciales inválidas";
                    problemDetails.Detail = invalidCredentialsException.Message;
                    break;
                case UserInactiveException userInactiveException:
                    problemDetails.Status = (int)HttpStatusCode.Forbidden;
                    problemDetails.Title = "Cuenta desactivada";
                    problemDetails.Detail = userInactiveException.Message;
                    break;
                case ConflictException conflictException:
                    problemDetails.Status = (int)HttpStatusCode.Conflict;
                    problemDetails.Title = "Conflicto";
                    problemDetails.Detail = conflictException.Message;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    problemDetails.Status = (int)HttpStatusCode.Unauthorized;
                    problemDetails.Title = "No autorizado";
                    problemDetails.Detail = unauthorizedAccessException.Message;
                    break;
                case InvalidOperationException invalidOperationException:
                    problemDetails.Status = (int)HttpStatusCode.BadRequest;
                    problemDetails.Title = "Operación no válida";
                    problemDetails.Detail = invalidOperationException.Message;
                    break;
            }

            return problemDetails;
        }
    }
}
