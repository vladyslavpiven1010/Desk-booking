using DeskBooking.Api.DTOs.Reservation;
using DeskBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.Api.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationsController(ReservationService reservationService)
        => _reservationService = reservationService;

    /// <summary>
    /// Создать бронь.
    /// POST /api/reservations
    /// body: { deskId, userId, startDate, endDate }
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateReservationDto dto)
    {
        var id = await _reservationService.CreateAsync(dto);
        return Ok(new { reservationId = id });
    }

    /// <summary>
    /// Отменить бронь: на день (split) или целиком.
    /// POST /api/reservations/{id}/cancel
    /// body: { userId, mode: Day|Range, day? }
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult> Cancel([FromRoute] Guid id, [FromBody] CancelReservationDto dto)
    {
        await _reservationService.CancelAsync(id, dto);
        return NoContent();
    }
}
