using DeskBooking.Api.Domain;

namespace DeskBooking.Api.DTOs.Desk;

public class DeskDto
{
    public Guid DeskId { get; set; }
    public int Number { get; set; }

    public DeskStatus Status { get; set; }

    public string? ReservedBy { get; set; }
    public Guid? ReservedByUserId { get; set; }

    public string? MaintenanceMessage { get; set; }

    public bool IsMine { get; set; }

    public Guid? ReservationId { get; set; }
}
