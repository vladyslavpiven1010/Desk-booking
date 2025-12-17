namespace DeskBooking.Api.DTOs.Reservation;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid DeskId { get; set; }
    public int DeskNumber { get; set; }

    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = "";

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool IsCanceled { get; set; }
    public DateTime? CanceledAt { get; set; }
}

