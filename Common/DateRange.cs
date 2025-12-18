namespace Desk_booking.Common;

/// <summary>
/// Value-object for data range.
/// </summary>
public readonly record struct DateRange(DateTime From, DateTime To)
{
    public DateRange Normalize()
    {
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
