using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleCRM.Domain.Entities;

namespace VehicleCRM.Infrastructure.Persistence.Configurations
{
    public class SaleOpportunityConfiguration : IEntityTypeConfiguration<SaleOpportunity>
    {
        public void Configure(EntityTypeBuilder<SaleOpportunity> builder)
        {
            builder.ToTable("SaleOpportunities");

            builder.HasKey(so => so.Id);

            builder.Property(so => so.CreateDate)
                .IsRequired();

            builder.Property(so => so.ModificationDate);

            builder.Property(so => so.DeleteDate);

            builder.Property(so => so.IsDeleted)
                .IsRequired();

            builder.Property(so => so.CustomerId)
                .IsRequired();

            builder.Property(so => so.VehicleId)
                .IsRequired();

            builder.Property(so => so.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(so => so.ProposedValue)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(so => so.Notes)
                .HasMaxLength(2000)
                .IsRequired();

            builder.HasOne(so => so.Customer)
                .WithMany()
                .HasForeignKey(so => so.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(so => so.Vehicle)
                .WithMany()
                .HasForeignKey(so => so.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
