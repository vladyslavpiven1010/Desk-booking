using DeskBooking.Api.DTOs.Profile;
using DeskBooking.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskBooking.Api.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
        => _profileService = profileService;

    /// <summary>
    /// Получить профиль пользователя:
    /// GET /api/profile?userId=...
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ProfileDto>> GetProfile([FromQuery] Guid userId)
    {
        var profile = await _profileService.GetProfileAsync(userId);
        return Ok(profile);
    }
}

