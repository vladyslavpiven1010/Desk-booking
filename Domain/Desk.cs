namespace DeskBooking.Api.Domain;

public class Desk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; set; }
}
