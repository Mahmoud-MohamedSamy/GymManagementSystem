using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class TrainerConfiguration : GymUserConfiguration<Trainer>
{
    public override void Configure(EntityTypeBuilder<Trainer> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CreatedAt)
               .HasColumnName("HireDate")
               .HasDefaultValueSql("GETDATE()");

        builder.ToTable("Trainers", t =>
        {
            t.HasCheckConstraint("TrainerEmailCheck", EmailCheck);
            t.HasCheckConstraint("TrainerPhoneCheck", PhoneCheck);
        });
    }
}
