using DeskBooking.Api.Common;
using DeskBooking.Api.Data;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Reservation;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Services;

/// <summary>
/// Сервис бронирований: создать / отменить (range или один день).
/// Здесь ключевая сложность: отмена на один день => сдвиг или split брони.
/// </summary>
public class ReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db) => _db = db;

    public async Task<Guid> CreateAsync(CreateReservationDto dto)
    {
        var range = new DateRange(dto.StartDate, dto.EndDate).Normalize();

        // Проверяем существование пользователя и стола (лучше сразу отдать 404)
        var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
        if (!userExists) throw new BusinessException("User not found.", 404);

        var deskExists = await _db.Desks.AnyAsync(d => d.Id == dto.DeskId);
        if (!deskExists) throw new BusinessException("Desk not found.", 404);

        // 1) Запрещаем бронь на maintenance
        var hasMaintenance = await _db.MaintenanceWindows
            .AnyAsync(m => m.DeskId == dto.DeskId
                        && m.StartDate.Date <= range.To
                        && m.EndDate.Date >= range.From);

        if (hasMaintenance)
            throw new BusinessException("Desk is under maintenance for the selected date range.", 409);

        // 2) Запрещаем пересечение с активной бронью
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
        // Вытягиваем бронь
        var reservation = await _db.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null) throw new BusinessException("Reservation not found.", 404);
        if (reservation.IsCanceled) return; // идемпотентность

        // “Без auth” всё равно нужно правило: отменить может только владелец брони.
        if (reservation.UserId != dto.UserId)
            throw new BusinessException("You can cancel only your own reservation.", 403);

        if (dto.Mode == CancelMode.Range)
        {
            // Отмена всей брони => soft-cancel (для истории)
            reservation.CanceledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return;
        }

        // CancelMode.Day
        if (dto.Day == null)
            throw new BusinessException("Day is required when cancel mode is 'Day'.", 400);

        var day = dto.Day.Value.Date;

        // День должен быть внутри брони
        if (!reservation.Range.ContainsDay(day))
            throw new BusinessException("The specified day is outside the reservation range.", 400);

        // Если бронь на один день => просто cancel
        if (reservation.StartDate.Date == reservation.EndDate.Date)
        {
            reservation.CanceledAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return;
        }

        // Если день = StartDate => сдвигаем начало на +1
        if (day == reservation.StartDate.Date)
        {
            reservation.StartDate = reservation.StartDate.Date.AddDays(1);
            await _db.SaveChangesAsync();
            return;
        }

        // Если день = EndDate => сдвигаем конец на -1
        if (day == reservation.EndDate.Date)
        {
            reservation.EndDate = reservation.EndDate.Date.AddDays(-1);
            await _db.SaveChangesAsync();
            return;
        }

        // Иначе: день внутри => split на две брони
        // Левый кусок: [Start .. day-1]
        var leftStart = reservation.StartDate.Date;
        var leftEnd = day.AddDays(-1);

        // Правый кусок: [day+1 .. End]
        var rightStart = day.AddDays(1);
        var rightEnd = reservation.EndDate.Date;

        // Мы обновим текущую бронь как левую часть, а правую создадим новой.
        // Это проще, чем отменять и создавать две.
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
