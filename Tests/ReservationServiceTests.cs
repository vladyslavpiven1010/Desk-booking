using Desk_booking.Common;
using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Reservation;
using DeskBooking.Api.Services;
using FluentAssertions;
using Xunit;

namespace DeskBooking.Api.Tests;

public class ReservationServiceTests
{
    [Fact]
    public async Task CreateReservation_ShouldFail_WhenDatesIntersect()
    {
        // Arrange
        var db = TestDbFactory.Create();
        var service = new ReservationService(db);

        var user = new User { FirstName = "Test", LastName = "User" };
        var desk = new Desk { Number = 1 };

        db.Users.Add(user);
        db.Desks.Add(desk);

        db.Reservations.Add(new Reservation
        {
            DeskId = desk.Id,
            UserId = user.Id,
            StartDate = new DateTime(2025, 1, 10),
            EndDate = new DateTime(2025, 1, 12)
        });

        db.SaveChanges();

        var dto = new CreateReservationDto
        {
            DeskId = desk.Id,
            UserId = user.Id,
            StartDate = new DateTime(2025, 1, 11),
            EndDate = new DateTime(2025, 1, 13)
        };

        // Act
        Func<Task> act = async () => await service.CreateAsync(dto);

        // Assert
        await act.Should().ThrowAsync<BusinessException>()
            .WithMessage("*already reserved*");
    }

    [Fact]
    public async Task CancelReservation_ForOneDay_ShouldSplitReservation()
    {
        // Arrange
        var db = TestDbFactory.Create();
        var service = new ReservationService(db);

        var user = new User { FirstName = "Alice", LastName = "Test" };
        var desk = new Desk { Number = 5 };

        db.Users.Add(user);
        db.Desks.Add(desk);

        var reservation = new Reservation
        {
            DeskId = desk.Id,
            UserId = user.Id,
            StartDate = new DateTime(2025, 1, 10),
            EndDate = new DateTime(2025, 1, 14)
        };

        db.Reservations.Add(reservation);
        db.SaveChanges();

        // Act
        await service.CancelAsync(reservation.Id, new CancelReservationDto
        {
            UserId = user.Id,
            Mode = CancelMode.Day,
            Day = new DateTime(2025, 1, 12)
        });

        // Assert
        var reservations = db.Reservations.ToList();

        reservations.Should().HaveCount(2);
        reservations.Should().Contain(r =>
            r.StartDate == new DateTime(2025, 1, 10) &&
            r.EndDate == new DateTime(2025, 1, 11));

        reservations.Should().Contain(r =>
            r.StartDate == new DateTime(2025, 1, 13) &&
            r.EndDate == new DateTime(2025, 1, 14));
    }
}

