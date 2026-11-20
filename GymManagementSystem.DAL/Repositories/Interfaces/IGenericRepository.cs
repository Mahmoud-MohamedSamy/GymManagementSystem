using System.Linq.Expressions;
using GymManagementSystem.DAL.Entities;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? condition = null);
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    bool Any(Expression<Func<T, bool>> condition);
    int Count(Expression<Func<T, bool>>? condition = null);
}
