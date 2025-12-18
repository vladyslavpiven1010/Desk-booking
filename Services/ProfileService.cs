using AutoMapper;
using Desk_booking.Common;
using DeskBooking.Api.Data;
using DeskBooking.Api.DTOs.Profile;
using DeskBooking.Api.DTOs.Reservation;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Services;

public class ProfileService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ProfileService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ProfileDto> GetProfileAsync(Guid userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) throw new BusinessException("User not found.", 404);

        var today = DateTime.UtcNow.Date;

        var reservations = await _db.Reservations
            .Include(r => r.Desk)
            .Include(r => r.User)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        // "Current" — still relevant (not canceled) and end >= today
        var current = reservations
            .Where(r => r.CanceledAt == null && r.EndDate.Date >= today)
            .Select(r => _mapper.Map<ReservationDto>(r))
            .ToList();

        // "Past" — everything else: cancelled or already ended
        var past = reservations
            .Where(r => r.CanceledAt != null || r.EndDate.Date < today)
            .Select(r => _mapper.Map<ReservationDto>(r))
            .ToList();

        return new ProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            CurrentReservations = current,
            PastReservations = past
        };
    }
}

