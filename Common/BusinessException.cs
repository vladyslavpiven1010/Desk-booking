namespace Desk_booking.Common;

public class BusinessException : Exception
{
    public int StatusCode { get; }

    public BusinessException(string message, int statusCode = 409) : base(message)
    {
        StatusCode = statusCode;
    }
}

