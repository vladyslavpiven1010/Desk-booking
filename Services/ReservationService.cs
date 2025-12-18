using Desk_booking.Common;
using DeskBooking.Api.Data;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Reservation;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Services;

/// <summary>
/// Booking service: create / cancel (range or single day).
/// The key difficulty here is: canceling for one day => shifting or splitting the booking.
/// </summary>
public class ReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db) => _db = db;

    public async Task<Guid> CreateAsync(CreateReservationDto dto)
    {
        var range = new DateRange(dto.StartDate, dto.EndDate).Normalize();

        // Check the existence of the user and table
        var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists) throw new BusinessException("User not found.", 404);

        var deskExists = await _db.Desks.AnyAsync(d => d.Id == dto.DeskId);
        if (!deskExists) throw new BusinessException("Desk not found.", 404);

        // 1) We prohibit reservations for maintenance
        var hasMaintenance = await _db.MaintenanceWindows
            .AnyAsync(m => m.DeskId == dto.DeskId
                        && m.StartDate.Date <= range.To
                        && m.EndDate.Date >= range.From);

        if (hasMaintenance)
            throw new BusinessException("Desk is under maintenance for the selected date range.", 409);

        // 2) We prohibit intersection with active booking
        var intersectsReservation = await _db.Reservations
            .Where(r => r.DeskId == dto.DeskId && r.CanceledAt == null)
            .AnyAsync(r => r.StartDate.Date <= range.To && r.EndDate.Date >= range.From);

        if (intersectsReservation)
            throw new BusinessException("Desk is already reserved for the selected date range.", 409);

        var reservation = new Reservation
        {
            DeskId = dto.DeskId,
            UserId = dto.UserId,
            StartDate = range.From,
            EndDate = range.To,
            CreatedAt = DateTime.UtcNow
        };

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();

        return reservation.Id;
    }

    public async Task CancelAsync(Guid reservationId, CancelReservationDto dto)
    {
        // Extract the reservation
        var reservation = await _db.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null) throw new BusinessException("Reservation not found.", 404);
        if (reservation.IsCanceled) return;

        // Only the booking owner can cancel.
        if (reservation.UserId != dto.UserId)
            throw new BusinessException("You can cancel only your own reservation.", 403);

        if (dto.Mode == CancelMode.Range)
        {
            reservation.CanceledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return;
        }

        // CancelMode.Day
        if (dto.Day == null)
            throw new BusinessException("Day is required when cancel mode is 'Day'.", 400);

        var day = dto.Day.Value.Date;

        if (!reservation.Range.ContainsDay(day))
            throw new BusinessException("The specified day is outside the reservation range.", 400);

        // If the reservation is for one day => just cancel
        if (reservation.StartDate.Date == reservation.EndDate.Date)
        {
            reservation.CanceledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return;
        }

        // If day = StartDate => shift the start by +1
        if (day == reservation.StartDate.Date)
        {
            reservation.StartDate = reservation.StartDate.Date.AddDays(1);
            await _db.SaveChangesAsync();
            return;
        }

        // If day = EndDate => shift the end by -1
        if (day == reservation.EndDate.Date)
        {
            reservation.EndDate = reservation.EndDate.Date.AddDays(-1);
            await _db.SaveChangesAsync();
            return;
        }

        // Otherwise: day inside => split into two reservations
        var leftStart = reservation.StartDate.Date;
        var leftEnd = day.AddDays(-1);

        // Right piece: [day+1 .. End]
        var rightStart = day.AddDays(1);
        var rightEnd = reservation.EndDate.Date;

        // Update the current reservation as the left part, and create a new one on the right.
        reservation.StartDate = leftStart;
        reservation.EndDate = leftEnd;

        var rightReservation = new Reservation
        {
            DeskId = reservation.DeskId,
            UserId = reservation.UserId,
            StartDate = rightStart,
            EndDate = rightEnd,
            CreatedAt = DateTime.UtcNow
        };

        _db.Reservations.Add(rightReservation);
        await _db.SaveChangesAsync();
    }
}
