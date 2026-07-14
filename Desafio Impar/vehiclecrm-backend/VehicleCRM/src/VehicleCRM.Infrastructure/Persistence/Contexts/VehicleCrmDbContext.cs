using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.Vehicles.Entities;

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

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var body = Expression.Equal(
                        Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                        Expression.Constant(false));
                    var lambda = Expression.Lambda(body, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

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
