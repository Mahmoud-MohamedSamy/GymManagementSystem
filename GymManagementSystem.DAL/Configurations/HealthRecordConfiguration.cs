using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
{
    public void Configure(EntityTypeBuilder<HealthRecord> builder)
    {
        builder.ToTable("Members");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Height).HasPrecision(6, 2);
        builder.Property(x => x.Weight).HasPrecision(6, 2);
        builder.Property(x => x.BloodType).HasColumnType("varchar").HasMaxLength(3);
        builder.Property(x => x.Note).HasMaxLength(500);
    }
}
