namespace DeskBooking.Api.DTOs.Desk;

public class DeskQueryDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public Guid CurrentUserId { get; set; }
}
