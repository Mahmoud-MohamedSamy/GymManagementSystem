namespace GymManagementSystem.DAL.Entities;

public class Member : GymUser
{
    public string? Photo { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
