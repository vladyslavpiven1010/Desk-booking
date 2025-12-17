using DeskBooking.Api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DeskBooking.Api.Data;

/// <summary>
/// EF Core DbContext. InMemory провайдер позволяет хранить данные только в памяти.
/// Для задания это идеально: быстро и не требует настоящей базы.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Desk> Desks => Set<Desk>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<MaintenanceWindow> MaintenanceWindows => Set<MaintenanceWindow>();
}
