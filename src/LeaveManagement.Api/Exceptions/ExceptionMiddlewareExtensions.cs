namespace LeaveManagement.Api.Exceptions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
