namespace ShipmentTracker.App.Application.Common.Exceptions;

public class BaseException: Exception
{
    public HttpStatusCode StatusCode { get; }

    public BaseException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : base(message)
    {
        StatusCode = httpStatusCode;
    }
}
