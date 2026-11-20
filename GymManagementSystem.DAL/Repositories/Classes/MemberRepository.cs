using GymManagementSystem.DAL.Data;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(GymDbContext context) : base(context)
    {
    }

    public Member? GetWithHealthRecord(int id)
        => _dbSet.Include(m => m.HealthRecord)
                 .FirstOrDefault(m => m.Id == id);
}
