namespace ShipmentTracker.App.API.Infrastructure.Exceptions;
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
                                IHostEnvironment environment,
                                IServiceScopeFactory serviceScopeFactory) : IExceptionHandler
{
    private const bool IsLastStopInPipline = true;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        problemDetails.Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}";
        if (exception is BaseException e)
        {
            httpContext.Response.StatusCode = (int)e.StatusCode;
            problemDetails.Title = e.Message;
        }
        else
        {
            problemDetails.Title = exception.Message;
        }
        logger.LogError(exception, $"Could not request on machine {Environment.MachineName} with trace Id {Activity.Current?.Id ?? httpContext.TraceIdentifier}");
        problemDetails.Status = httpContext.Response.StatusCode;
        problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        if (!environment.IsProduction())
        {
            problemDetails.Detail = exception.StackTrace;
        }
        await HandleExceptionLogAsync(httpContext, exception);

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return IsLastStopInPipline;
    }
    private async Task HandleExceptionLogAsync(HttpContext httpContext, Exception exception)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var exceptionLoggerService = scope.ServiceProvider.GetRequiredService<IExceptionLoggerService>();

        await exceptionLoggerService.ExceptionLoggerAsync(new ExceptionDTO(
            httpContext.User?.Identity?.Name ?? string.Empty,
            exception.GetType().Name,
            exception.Source ?? string.Empty,
            exception.Message,
            exception.StackTrace ?? string.Empty,
            Severity(httpContext.Response.StatusCode),
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty,
            exception.InnerException is not null ?
                             MapInnerException(exception.InnerException) :
                             new InnerExceptionDTO(Name: string.Empty,
                                                   Source: string.Empty,
                                                   Message: string.Empty,
                                                   StackTrace: string.Empty),
            DateTime.UtcNow));
    }
    private InnerExceptionDTO MapInnerException(Exception innerException)
    {
        return new InnerExceptionDTO(
            innerException.GetType().Name,
            innerException.Source ?? string.Empty,
            innerException.Message,
            innerException.StackTrace ?? string.Empty
        );
    }
    private string Severity(int statusCode)
    {
        if (statusCode >= 500) // Server errors (500-599)
        {
            return nameof(SeverityLevel.Critical);
        }

        if (statusCode >= 400 && statusCode < 500) // Client errors (400-499)
        {
            return nameof(SeverityLevel.Warning);
        }

        if (statusCode >= 200 && statusCode < 400) // Success (200-299) or Redirection (300-399)
        {
            return nameof(SeverityLevel.Info);
        }

        return nameof(SeverityLevel.Critical);
    }
}
