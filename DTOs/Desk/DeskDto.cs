using DeskBooking.Api.Domain;

namespace DeskBooking.Api.DTOs.Desk;

/// <summary>
/// DTO для карточки стола на UI.
/// Тут есть всё, чтобы показать цвет и подсказку на hover.
/// </summary>
public class DeskDto
{
    public Guid DeskId { get; set; }
    public int Number { get; set; }

    public DeskStatus Status { get; set; }

    // Tooltip для Reserved
    public string? ReservedBy { get; set; }
    public Guid? ReservedByUserId { get; set; }

    // Tooltip для Maintenance
    public string? MaintenanceMessage { get; set; }

    // Нужно, чтобы показать кнопки отмены, если бронь моя
    public bool IsMine { get; set; }

    // Если Reserved — отдадим reservationId, чтобы можно было отменять
    public Guid? ReservationId { get; set; }
}
