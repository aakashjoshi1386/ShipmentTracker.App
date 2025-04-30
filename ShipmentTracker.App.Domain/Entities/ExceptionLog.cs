namespace ShipmentTracker.App.Domain.Entities;

public class ExceptionLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public InnerException InnerException { get; set; } = new InnerException();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public sealed class InnerException
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;
}
