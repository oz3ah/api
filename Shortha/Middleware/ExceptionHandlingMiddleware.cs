using System.Net;
using Shortha.Application.Exceptions;
using Shortha.Domain.Dto;

namespace Shortha.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment env)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            ErrorResponse error;
            int statusCode;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier);
                    break;

                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    error = ErrorResponse.From(validationEx.Message, validationEx.Errors, context.TraceIdentifier);
                    break;

                case ConflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    error = ErrorResponse.From(ex.Message, traceId: context.TraceIdentifier);
                    break;

                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    var message = env.IsDevelopment() ? ex.Message : "Internal Server Error";
                    var stack = env.IsDevelopment() ? new List<string> { ex.StackTrace ?? "" } : null;
                    error = ErrorResponse.From(message, stack, context.TraceIdentifier);
                    break;
            }

            response.StatusCode = statusCode;
            await response.WriteAsJsonAsync(error);
        }
    }

}
