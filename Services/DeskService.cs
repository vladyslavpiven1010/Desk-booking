using Desk_booking.Common;
using DeskBooking.Api.Data;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Desk;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Services;

/// <summary>
/// Table view service.
/// Main task: return a list of tables with statuses and tooltip data for a selected date range.
/// All logic is on the backend (as required by the task).
/// </summary>
public class DeskService
{
    private readonly AppDbContext _db;

    public DeskService(AppDbContext db) => _db = db;

    public async Task<List<DeskDto>> GetDesksAsync(DeskQueryDto query)
    {
        var range = new DateRange(query.From, query.To).Normalize();

        var desks = await _db.Desks
            .OrderBy(d => d.Number)
            .ToListAsync();

        // To avoid N+1, we take the relevant maintenance/reservations in advance,
        // which intersect with the range:
        var maintenance = await _db.MaintenanceWindows
            .Where(m => m.StartDate.Date <= range.To && m.EndDate.Date >= range.From)
            .ToListAsync();

        var reservations = await _db.Reservations
            .Include(r => r.User)
            .Where(r => r.CanceledAt == null)
            .Where(r => r.StartDate.Date <= range.To && r.EndDate.Date >= range.From)
            .ToListAsync();

        // Collect DTO for each table
        var result = new List<DeskDto>(desks.Count);

        foreach (var desk in desks)
        {
            // 1) Maintenance takes precedence over reservations (logically: if a table is under repair, it is unavailable)
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

            // 2) We look for a reservation that intersects with the range
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

            // 3) Or free
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
