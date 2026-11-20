using GymManagementSystem.DAL.Data;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    private IMemberRepository? _memberRepository;
    private ISessionRepository? _sessionRepository;

    public UnitOfWork(GymDbContext context)
    {
        _context = context;
    }

    public IMemberRepository MemberRepository
        => _memberRepository ??= new MemberRepository(_context);

    public ISessionRepository SessionRepository
        => _sessionRepository ??= new SessionRepository(_context);

    public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (!_repositories.TryGetValue(type, out var repository))
        {
            repository = new GenericRepository<T>(_context);
            _repositories[type] = repository;
        }
        return (IGenericRepository<T>)repository;
    }

    public int SaveChanges() => _context.SaveChanges();
}
