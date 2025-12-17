using DeskBooking.Api.Common;

namespace DeskBooking.Api.Domain;

public class Reservation
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DeskId { get; set; }
    public Desk? Desk { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public DateTime StartDate { get; set; }   // включительно
    public DateTime EndDate { get; set; }     // включительно

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Если отменили бронь (whole range или исходную при split) — ставим дату.
    /// Это нужно, чтобы хранить историю.
    /// </summary>
    public DateTime? CanceledAt { get; set; }

    public bool IsCanceled => CanceledAt != null;

    public DateRange Range => new DateRange(StartDate.Date, EndDate.Date);

    public bool Intersects(DateRange range) => Range.Intersects(range);

    public bool IsActiveAt(DateTime day)
        => !IsCanceled && day.Date >= StartDate.Date && day.Date <= EndDate.Date;
}