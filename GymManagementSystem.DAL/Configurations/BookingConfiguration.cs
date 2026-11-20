using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Ignore(x => x.Id);
        builder.HasKey(x => new { x.MemberId, x.SessionId });

        builder.Property(x => x.CreatedAt)
               .HasColumnName("BookingDate")
               .HasDefaultValueSql("GETDATE()");

        builder.Ignore(x => x.BookingDate);

        builder.HasOne(x => x.Member)
               .WithMany(m => m.Bookings)
               .HasForeignKey(x => x.MemberId);

        builder.HasOne(x => x.Session)
               .WithMany(s => s.Bookings)
               .HasForeignKey(x => x.SessionId);
    }
}
