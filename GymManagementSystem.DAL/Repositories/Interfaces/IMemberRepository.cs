using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IMemberRepository : IGenericRepository<Member>
{
    Member? GetWithHealthRecord(int id);
}
