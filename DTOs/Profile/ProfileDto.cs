using DeskBooking.Api.DTOs.Reservation;

namespace DeskBooking.Api.DTOs.Profile;

public class ProfileDto
{
    public Guid UserId { get; set; }
    public string FullName { get; set; } = "";

    public List<ReservationDto> CurrentReservations { get; set; } = [];
    public List<ReservationDto> PastReservations { get; set; } = [];
}
