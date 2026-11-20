using GymManagementSystem.DAL.Data;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(GymDbContext context) : base(context)
    {
    }

    public IEnumerable<Session> GetAllWithTrainerAndCategory()
        => _dbSet.AsNoTracking()
                 .Include(s => s.Trainer)
                 .Include(s => s.Category)
                 .Include(s => s.Bookings)
                 .OrderBy(s => s.StartDate)
                 .ToList();

    public Session? GetByIdWithTrainerAndCategory(int id)
        => _dbSet.Include(s => s.Trainer)
                 .Include(s => s.Category)
                 .Include(s => s.Bookings)
                 .FirstOrDefault(s => s.Id == id);

    public bool TrainerHasOverlap(int trainerId, DateTime start, DateTime end, int? excludeSessionId = null)
        => _dbSet.Any(s => s.TrainerId == trainerId
                           && (excludeSessionId == null || s.Id != excludeSessionId)
                           && s.StartDate < end
                           && s.EndDate > start);
}
