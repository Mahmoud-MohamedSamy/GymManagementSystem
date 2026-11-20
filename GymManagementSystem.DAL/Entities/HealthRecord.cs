namespace GymManagementSystem.DAL.Entities;

public class HealthRecord
{
    public int Id { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string BloodType { get; set; } = null!;
    public string? Note { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;

    public Member Member { get; set; } = null!;
}
