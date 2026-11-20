using System.Linq.Expressions;
using GymManagementSystem.DAL.Data;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly GymDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(GymDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? condition = null)
    {
        IQueryable<T> query = _dbSet.AsNoTracking();
        if (condition is not null)
            query = query.Where(condition);
        return query.ToList();
    }

    public T? GetById(int id) => _dbSet.Find(id);

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);

    public bool Any(Expression<Func<T, bool>> condition) => _dbSet.Any(condition);

    public int Count(Expression<Func<T, bool>>? condition = null)
        => condition is null ? _dbSet.Count() : _dbSet.Count(condition);
}
