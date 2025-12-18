using DeskBooking.Api.Domain;

namespace DeskBooking.Api.Data;

public static class SeedData
{
    public static void Seed(AppDbContext db)
    {
        if (db.Desks.Any()) return;

        var user1 = new User { FirstName = "Alice", LastName = "Johnson" };
        var user2 = new User { FirstName = "Bob", LastName = "Smith" };

        var desks = Enumerable.Range(1, 12)
            .Select(n => new Desk { Number = n })
            .ToList();

        db.Users.AddRange(user1, user2);
        db.Desks.AddRange(desks);

        // Example: table 3 has maintenance for several days
        db.MaintenanceWindows.Add(new MaintenanceWindow
        {
            DeskId = desks[2].Id, // №3
            StartDate = DateTime.UtcNow.Date.AddDays(2),
            EndDate = DateTime.UtcNow.Date.AddDays(4),
            Message = "Broken monitor"
        });

        // Example: table 5 has maintenance for several days
        db.Reservations.Add(new Reservation
        {
            DeskId = desks[4].Id,
            UserId = user1.Id,
            StartDate = DateTime.UtcNow.Date.AddDays(1),
            EndDate = DateTime.UtcNow.Date.AddDays(3)
        });

        db.SaveChanges();
    }
}
