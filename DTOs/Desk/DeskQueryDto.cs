namespace DeskBooking.Api.DTOs.Desk;

/// <summary>
/// Query DTO. Без auth фронт будет передавать currentUserId (выбранный пользователь).
/// from/to — выбранный диапазон дат.
/// </summary>
public class DeskQueryDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public Guid CurrentUserId { get; set; }
}
