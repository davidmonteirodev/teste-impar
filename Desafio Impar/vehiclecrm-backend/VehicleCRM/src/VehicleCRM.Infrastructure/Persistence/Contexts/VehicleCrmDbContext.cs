using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Entities.Base;

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

        public override int SaveChanges()
        {
            ApplyBaseEntityAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyBaseEntityAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyBaseEntityAudit()
        {
            var utcNow = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Property(entity => entity.CreateDate).CurrentValue = utcNow;
                        entry.Property(entity => entity.ModificationDate).CurrentValue = null;
                        entry.Property(entity => entity.DeleteDate).CurrentValue = null;
                        entry.Property(entity => entity.IsDeleted).CurrentValue = false;
                        break;

                    case EntityState.Modified:
                        entry.Property(entity => entity.ModificationDate).CurrentValue = utcNow;
                        entry.Property(entity => entity.CreateDate).IsModified = false;
                        entry.Property(entity => entity.DeleteDate).IsModified = false;
                        entry.Property(entity => entity.IsDeleted).IsModified = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Property(entity => entity.ModificationDate).CurrentValue = utcNow;
                        entry.Property(entity => entity.DeleteDate).CurrentValue = utcNow;
                        entry.Property(entity => entity.IsDeleted).CurrentValue = true;
                        break;
                }
            }
        }
    }
}
