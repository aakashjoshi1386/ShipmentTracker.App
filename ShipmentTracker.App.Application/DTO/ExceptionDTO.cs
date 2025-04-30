namespace ShipmentTacker.App.Application.DTO;
public sealed record ExceptionDTO(string UserId,
                           string Name,
                           string Source,
                           string Message,
                           string StackTrace,
                           string Severity,
                           string Environment,
                           InnerExceptionDTO InnerException,
                           DateTime CreatedAt);
public sealed record InnerExceptionDTO(
                               string Name,
                               string Source,
                               string Message,
                               string StackTrace);