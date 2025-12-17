namespace DeskBooking.Api.DTOs.Reservation;

public class CreateReservationDto
{
    public Guid DeskId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }  // включительно
    public DateTime EndDate { get; set; }    // включительно
}
