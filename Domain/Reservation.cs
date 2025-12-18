using Desk_booking.Common;

namespace DeskBooking.Api.Domain;

public class Reservation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DeskId { get; set; }
    public Desk? Desk { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CanceledAt { get; set; }

    public bool IsCanceled => CanceledAt != null;

    public DateRange Range => new DateRange(StartDate.Date, EndDate.Date);

    public bool Intersects(DateRange range) => Range.Intersects(range);

    public bool IsActiveAt(DateTime day)
        => !IsCanceled && day.Date >= StartDate.Date && day.Date <= EndDate.Date;
}