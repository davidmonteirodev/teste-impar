using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.ValueObjects;

namespace VehicleCRM.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreateDate)
                .IsRequired();

            builder.Property(c => c.ModificationDate);

            builder.Property(c => c.DeleteDate);

            builder.Property(c => c.IsDeleted)
                .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(150)
                .IsRequired()
                .HasConversion(
                    email => email.Value,
                    value => Email.Create(value));

            builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("IX_Customers_Email_Unique");

            builder.Property(c => c.Phone)
                .HasMaxLength(30)
                .IsRequired()
                .HasConversion(
                    phone => phone.Value,
                    value => Phone.Create(value));

            builder.Property(c => c.MainInterest)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
