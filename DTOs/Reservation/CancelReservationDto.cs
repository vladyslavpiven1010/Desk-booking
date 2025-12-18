using DeskBooking.Api.Domain;

namespace DeskBooking.Api.DTOs.Reservation;

public class CancelReservationDto
{
    public Guid UserId { get; set; }

    public CancelMode Mode { get; set; }

    public DateTime? Day { get; set; }
}
