using GymManagementSystem.DAL.Entities.Enums;

namespace GymManagementSystem.DAL.Entities;

public class Trainer : GymUser
{
    public Specialties Specialties { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
