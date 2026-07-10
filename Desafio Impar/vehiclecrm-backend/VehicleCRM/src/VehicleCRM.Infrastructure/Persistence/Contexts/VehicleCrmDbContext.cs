using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Entities;

namespace VehicleCRM.Infrastructure.Persistence.Contexts
{
    public class VehicleCrmDbContext : DbContext
    {
        public VehicleCrmDbContext(DbContextOptions<VehicleCrmDbContext> options)
            : base(options) { }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<SaleOpportunity> SaleOpportunities => Set<SaleOpportunity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleCrmDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
