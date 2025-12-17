using DeskBooking.Api.DTOs.Desk;
using DeskBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.Api.Controllers;

[ApiController]
[Route("api/desks")]
public class DesksController : ControllerBase
{
    private readonly DeskService _deskService;

    public DesksController(DeskService deskService) => _deskService = deskService;

    /// <summary>
    /// Возвращает список столов на диапазон дат, со статусом и tooltip-данными.
    /// Пример:
    /// GET /api/desks?from=2025-12-17&to=2025-12-20&currentUserId=...
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<DeskDto>>> GetDesks([FromQuery] DeskQueryDto query)
    {
        var desks = await _deskService.GetDesksAsync(query);
        return Ok(desks);
    }
}
