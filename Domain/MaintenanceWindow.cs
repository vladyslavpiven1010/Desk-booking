using DeskBooking.Api.Common;

namespace DeskBooking.Api.Domain;

public class MaintenanceWindow
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid DeskId { get; set; }
    public Desk? Desk { get; set; }

    public DateTime StartDate { get; set; }   // включительно
    public DateTime EndDate { get; set; }     // включительно

    public string Message { get; set; } = "Maintenance";

    public DateRange Range => new DateRange(StartDate.Date, EndDate.Date);

    public bool Intersects(DateRange range) => Range.Intersects(range);
}
