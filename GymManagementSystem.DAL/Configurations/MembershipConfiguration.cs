using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
{
    public void Configure(EntityTypeBuilder<Membership> builder)
    {
        builder.Ignore(x => x.Id);
        builder.HasKey(x => new { x.MemberId, x.PlanId });

        builder.Property(x => x.CreatedAt)
               .HasColumnName("StartDate")
               .HasDefaultValueSql("GETDATE()");

        builder.Ignore(x => x.StartDate);
        builder.Ignore(x => x.Status);

        builder.HasOne(x => x.Member)
               .WithMany(m => m.Memberships)
               .HasForeignKey(x => x.MemberId);

        builder.HasOne(x => x.Plan)
               .WithMany(p => p.Memberships)
               .HasForeignKey(x => x.PlanId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
