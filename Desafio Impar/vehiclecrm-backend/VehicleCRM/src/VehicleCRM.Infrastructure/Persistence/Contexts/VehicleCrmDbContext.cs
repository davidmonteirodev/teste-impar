using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Entities;

namespace VehicleCRM.Infrastructure.Persistence.Contexts
{
    public class VehicleCrmDbContext : DbContext
    {
        public VehicleCrmDbContext(DbContextOptions<VehicleCrmDbContext> options)
            : base(options) { }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VehicleCrmDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}