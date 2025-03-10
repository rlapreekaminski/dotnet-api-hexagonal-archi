using LeaveManagement.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace LeaveManagement.Api.Exceptions;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (HttpStatusCode statusCode, string message) = exception switch
        {
            InvalidLeaveRequestException => (HttpStatusCode.BadRequest, exception.Message),
            RepositoryNotFoundException => (HttpStatusCode.NotFound, exception.Message),
            RepositoryException => (HttpStatusCode.InternalServerError, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        ErrorDetails errorDetails = new(context.Response.StatusCode, message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
    }
}
