using System.Reflection;
using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Data;

public class GymDbContext : IdentityDbContext<ApplicationUser>
{
    public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Trainer> Trainers => Set<Trainer>();
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<Booking> Bookings => Set<Booking>();

    public override int SaveChanges()
    {
        StampUpdatedAt();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        StampUpdatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void StampUpdatedAt()
    {
        var modified = ChangeTracker.Entries<BaseEntity>()
                                    .Where(e => e.State == EntityState.Modified);
        foreach (var entry in modified)
            entry.Entity.UpdatedAt = DateTime.Now;
    }
}
