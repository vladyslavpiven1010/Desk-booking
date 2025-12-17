namespace DeskBooking.Api.Common;

/// <summary>
/// Value-object для диапазона дат.
/// Мы храним даты как DateOnly (если хочешь) или DateTime.
/// Для простоты использую DateTime, но трактуем как "день" без времени.
/// </summary>
public readonly record struct DateRange(DateTime From, DateTime To)
{
    public DateRange Normalize()
    {
        // Убираем время, чтобы логика "по дням" была предсказуемой.
        var from = From.Date;
        var to = To.Date;

        if (to < from)
            throw new BusinessException("Invalid date range: 'to' is earlier than 'from'.", 400);

        return new DateRange(from, to);
    }

    public bool Intersects(DateRange other)
        => From <= other.To && To >= other.From;

    public bool ContainsDay(DateTime day)
        => day.Date >= From.Date && day.Date <= To.Date;
}
