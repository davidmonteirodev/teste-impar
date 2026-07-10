using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleCRM.Domain.Entities;

namespace VehicleCRM.Infrastructure.Persistence.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.PublicId)
                .IsRequired();

            builder.Property(v => v.CreateDate)
                .IsRequired();

            builder.Property(v => v.ModificationDate);

            builder.Property(v => v.IsDeleted)
                .IsRequired();

            builder.Property(v => v.Brand)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(v => v.Model)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(v => v.Year)
                .IsRequired();

            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(v => v.Color)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(v => v.Mileage)
                .IsRequired();

            builder.Property(v => v.Status)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
