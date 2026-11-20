using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public abstract class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
{
    protected const string EmailCheck = "Email LIKE '_%@_%._%'";
    protected const string PhoneCheck =
        "Phone LIKE '01[0125][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'";

    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Name)
               .HasColumnType("varchar")
               .HasMaxLength(50);

        builder.Property(x => x.Email)
               .HasColumnType("varchar")
               .HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(11);
        builder.HasIndex(x => x.Phone).IsUnique();

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

        builder.OwnsOne(x => x.Address, address =>
        {
            address.Property(a => a.BuildingNumber).HasColumnName("BuildingNumber");
            address.Property(a => a.Street)
                   .HasColumnName("Street")
                   .HasColumnType("varchar")
                   .HasMaxLength(30);
            address.Property(a => a.City)
                   .HasColumnName("City")
                   .HasColumnType("varchar")
                   .HasMaxLength(30);
        });
    }
}
