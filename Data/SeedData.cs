using DeskBooking.Api.Domain;

namespace DeskBooking.Api.Data;

public static class SeedData
{
    public static void Seed(AppDbContext db)
    {
        if (db.Desks.Any()) return;

        var user1 = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), FirstName = "Alice", LastName = "Johnson" };
        var user2 = new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), FirstName = "Bob", LastName = "Smith" };

        var desks = Enumerable.Range(1, 12)
            .Select(n => new Desk { Number = n })
            .ToList();

        db.Users.AddRange(user1, user2);
        db.Desks.AddRange(desks);

        // Пример: у стола 3 maintenance на несколько дней
        db.MaintenanceWindows.Add(new MaintenanceWindow
        {
            DeskId = desks[2].Id, // №3
            StartDate = DateTime.UtcNow.Date.AddDays(2),
            EndDate = DateTime.UtcNow.Date.AddDays(4),
            Message = "Broken monitor"
        });

        // Пример: бронь на стол №5
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
