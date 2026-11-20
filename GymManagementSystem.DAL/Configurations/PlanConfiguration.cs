using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50);

        builder.Property(x => x.Description)
               .HasColumnType("varchar")
               .HasMaxLength(200);

        builder.Property(x => x.Price).HasPrecision(10, 2);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("PlanDurationCheck", "DurationDays BETWEEN 1 AND 365");
        });
    }
}
