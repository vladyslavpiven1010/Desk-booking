using DeskBooking.Api.Domain;
using DeskBooking.Api.DTOs.Desk;
using DeskBooking.Api.Services;
using FluentAssertions;
using Xunit;

namespace DeskBooking.Api.Tests;

public class DeskServiceTests
{
    [Fact]
    public async Task Desk_ShouldBeMaintenance_WhenMaintenanceOverlapsRange()
    {
        // Arrange
        var db = TestDbFactory.Create();
        var service = new DeskService(db);

        var desk = new Desk { Number = 3 };
        db.Desks.Add(desk);

        db.MaintenanceWindows.Add(new MaintenanceWindow
        {
            DeskId = desk.Id,
            StartDate = new DateTime(2025, 1, 10),
            EndDate = new DateTime(2025, 1, 12),
            Message = "Repair"
        });

        db.SaveChanges();

        // Act
        var result = await service.GetDesksAsync(new DeskQueryDto
        {
            From = new DateTime(2025, 1, 11),
            To = new DateTime(2025, 1, 13),
            CurrentUserId = Guid.NewGuid()
        });

        // Assert
        result.Single().Status.Should().Be(DeskStatus.Maintenance);
        result.Single().MaintenanceMessage.Should().Be("Repair");
    }
}

