namespace ShipmentTacker.App.Application.Abstraction;
public interface IExceptionLoggerService
{
    Task ExceptionLoggerAsync(ExceptionDTO exception);
}
