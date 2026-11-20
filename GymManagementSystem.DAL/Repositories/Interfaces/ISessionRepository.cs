using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface ISessionRepository : IGenericRepository<Session>
{
    IEnumerable<Session> GetAllWithTrainerAndCategory();
    Session? GetByIdWithTrainerAndCategory(int id);

    bool TrainerHasOverlap(int trainerId, DateTime start, DateTime end, int? excludeSessionId = null);
}
