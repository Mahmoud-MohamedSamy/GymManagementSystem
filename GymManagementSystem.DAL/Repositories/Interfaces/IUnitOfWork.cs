using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    IMemberRepository MemberRepository { get; }
    ISessionRepository SessionRepository { get; }

    IGenericRepository<T> GetRepository<T>() where T : BaseEntity;

    int SaveChanges();
}
