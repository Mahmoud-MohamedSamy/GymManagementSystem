using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class MemberConfiguration : GymUserConfiguration<Member>
{
    public override void Configure(EntityTypeBuilder<Member> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CreatedAt)
               .HasColumnName("JoinDate")
               .HasDefaultValueSql("GETDATE()");

        builder.HasOne(x => x.HealthRecord)
               .WithOne(h => h.Member)
               .HasForeignKey<HealthRecord>(h => h.Id);
        builder.Navigation(x => x.HealthRecord).IsRequired();

        builder.ToTable("Members", t =>
        {
            t.HasCheckConstraint("MemberEmailCheck", EmailCheck);
            t.HasCheckConstraint("MemberPhoneCheck", PhoneCheck);
        });
    }
}
