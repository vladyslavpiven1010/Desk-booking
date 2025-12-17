using DeskBooking.Api.Domain;

namespace DeskBooking.Api.DTOs.Reservation;

public class CancelReservationDto
{
    public Guid UserId { get; set; }

    public CancelMode Mode { get; set; }

    /// <summary>
    /// Требуется только при Mode = Day.
    /// День должен лежать внутри брони.
    /// </summary>
    public DateTime? Day { get; set; }
}
