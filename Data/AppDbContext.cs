using DeskBooking.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Api.Data;

/// <summary>
/// EF Core DbContext. The InMemory provider allows storing data only in memory.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Desk> Desks => Set<Desk>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<MaintenanceWindow> MaintenanceWindows => Set<MaintenanceWindow>();
}
