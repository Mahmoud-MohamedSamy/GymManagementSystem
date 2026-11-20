using GymManagementSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.CategoryName)
               .HasColumnType("varchar")
               .HasMaxLength(20);

        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
    }
}
