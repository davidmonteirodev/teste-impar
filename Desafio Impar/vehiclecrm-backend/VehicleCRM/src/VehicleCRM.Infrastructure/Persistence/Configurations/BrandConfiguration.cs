using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleCRM.Domain.Entities;

namespace VehicleCRM.Infrastructure.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.PublicId)
                .IsRequired();

            builder.Property(b => b.CreateDate)
                .IsRequired();

            builder.Property(b => b.ModificationDate);

            builder.Property(b => b.IsDeleted)
                .IsRequired();

            builder.Property(b => b.CreateUserId)
                .IsRequired();

            builder.Property(b => b.ModificationUserId);

            builder.Property(b => b.DeleteUserId);

            builder.Property(b => b.Name)
                .HasMaxLength(100)
                .IsRequired(false);
        }
    }
}
