using DeskBooking.Api.Common;
using DeskBooking.Api.Data;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Desk;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Services;

/// <summary>
/// Сервис "просмотра столов".
/// Главная задача: на выбранный диапазон дат вернуть список столов со статусами и tooltip-данными.
/// Вся логика — на бэкенде (как требует задание).
/// </summary>
public class DeskService
{
    private readonly AppDbContext _db;

    public DeskService(AppDbContext db) => _db = db;

    public async Task<List<DeskDto>> GetDesksAsync(DeskQueryDto query)
    {
        var range = new DateRange(query.From, query.To).Normalize();

        // Берём столы
        var desks = await _db.Desks
            .OrderBy(d => d.Number)
            .ToListAsync();

        // Чтобы избежать N+1, заранее берём relevant maintenance/reservations,
        // которые пересекаются с диапазоном:
        var maintenance = await _db.MaintenanceWindows
            .Where(m => m.StartDate.Date <= range.To && m.EndDate.Date >= range.From)
            .ToListAsync();

        var reservations = await _db.Reservations
            .Include(r => r.User)
            .Where(r => r.CanceledAt == null)
            .Where(r => r.StartDate.Date <= range.To && r.EndDate.Date >= range.From)
            .ToListAsync();

        // Собираем DTO для каждого стола
        var result = new List<DeskDto>(desks.Count);

        foreach (var desk in desks)
        {
            // 1) Maintenance имеет приоритет над бронью (логично: если стол на ремонте — он недоступен)
            var m = maintenance.FirstOrDefault(x => x.DeskId == desk.Id && x.Intersects(range));

            if (m != null)
            {
                result.Add(new DeskDto
                {
                    DeskId = desk.Id,
                    Number = desk.Number,
                    Status = DeskStatus.Maintenance,
                    MaintenanceMessage = m.Message
                });
                continue;
            }

            // 2) Ищем бронь, пересекающуюся с диапазоном
            var r = reservations.FirstOrDefault(x => x.DeskId == desk.Id && x.Intersects(range));

            if (r != null)
            {
                result.Add(new DeskDto
                {
                    DeskId = desk.Id,
                    Number = desk.Number,
                    Status = DeskStatus.Reserved,
                    ReservedBy = r.User?.FullName,
                    ReservedByUserId = r.UserId,
                    IsMine = r.UserId == query.CurrentUserId,
                    ReservationId = r.Id
                });
                continue;
            }

            // 3) Иначе свободен
            result.Add(new DeskDto
            {
                DeskId = desk.Id,
                Number = desk.Number,
                Status = DeskStatus.Open
            });
        }

        return result;
    }
}
