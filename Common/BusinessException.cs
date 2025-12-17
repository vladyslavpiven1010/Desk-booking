namespace DeskBooking.Api.Common;

/// <summary>
/// Ошибка бизнес-правил (например: "стол уже занят", "пересечение дат").
/// Мы используем её, чтобы отдать корректный HTTP статус (обычно 409/400),
/// и понятное сообщение фронтенду.
/// </summary>
public class BusinessException : Exception
{
    public int StatusCode { get; }

    public BusinessException(string message, int statusCode = 409) : base(message)
    {
        StatusCode = statusCode;
    }
}

