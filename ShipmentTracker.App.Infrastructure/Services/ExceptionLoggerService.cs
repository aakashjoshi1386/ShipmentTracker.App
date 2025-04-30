namespace ShipmentTracker.App.Infrastructure.Services;
public class ExceptionLoggerService(ShipmentTrackerAppDBContext context) : IExceptionLoggerService
{
    private readonly ShipmentTrackerAppDBContext _context = context;
    public async Task ExceptionLoggerAsync(ExceptionDTO exception)
    {
        await _context.ExceptionLogs.AddAsync(new Domain.Entities.ExceptionLog
        {
            UserId = exception.UserId,
            Name = exception.Name,
            Source = exception.Source,
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            Severity = exception.Severity,
            Environment = exception.Environment,
            InnerException = new Domain.Entities.InnerException
            {
                Name = exception.InnerException.Name,
                Source = exception.InnerException.Source,
                Message = exception.InnerException.Message,
                StackTrace = exception.InnerException.StackTrace
            },
        });
    }
}
