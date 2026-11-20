namespace GymManagementSystem.DAL.Entities;

public class Membership : BaseEntity
{
    public int MemberId { get; set; }
    public Member Member { get; set; } = null!;

    public int PlanId { get; set; }
    public Plan Plan { get; set; } = null!;

    public DateTime EndDate { get; set; }
    public DateTime StartDate => CreatedAt;
    public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
}
