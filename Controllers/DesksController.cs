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
    /// Returns a list of tables for a given date range, with status and tooltip data.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<DeskDto>>> GetDesks([FromQuery] DeskQueryDto query)
    {
        var desks = await _deskService.GetDesksAsync(query);
        return Ok(desks);
    }
}
