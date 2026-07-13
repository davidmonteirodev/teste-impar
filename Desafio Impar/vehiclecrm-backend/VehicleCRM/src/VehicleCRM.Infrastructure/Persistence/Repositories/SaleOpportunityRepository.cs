using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories.Criteria;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class SaleOpportunityRepository : BaseRepository<SaleOpportunity>, ISaleOpportunityRepository
    {
        public SaleOpportunityRepository(VehicleCrmDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<SaleOpportunity> Items, int TotalCount)> GetPagedAsync(
            SaleOpportunitySearchCriteria criteria,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<SaleOpportunity>()
                .Include(so => so.Vehicle)
                .Include(so => so.Customer)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.CustomerName))
                query = query.Where(so => so.Customer.Name.Contains(criteria.CustomerName));

            if (!string.IsNullOrWhiteSpace(criteria.VehicleModel))
                query = query.Where(so => so.Vehicle.Model.Contains(criteria.VehicleModel));

            if (criteria.Status.HasValue)
                query = query.Where(so => so.Status == criteria.Status.Value);

            if (criteria.ProposedValueFrom.HasValue)
                query = query.Where(so => so.ProposedValue >= criteria.ProposedValueFrom.Value);

            if (criteria.ProposedValueTo.HasValue)
                query = query.Where(so => so.ProposedValue <= criteria.ProposedValueTo.Value);

            if (criteria.CreateDateFrom.HasValue)
                query = query.Where(so => so.CreateDate >= criteria.CreateDateFrom.Value);

            if (criteria.CreateDateTo.HasValue)
                query = query.Where(so => so.CreateDate <= criteria.CreateDateTo.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(so => so.Id)
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<SaleOpportunity> GetByIdWithRelationsAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<SaleOpportunity>()
                .Include(so => so.Vehicle)
                .Include(so => so.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(so => so.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByCustomerAndVehicleAsync(long customerId, long vehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<SaleOpportunity>()
                .AnyAsync(so => so.CustomerId == customerId && so.VehicleId == vehicleId, cancellationToken);
        }

        public async Task<bool> HasActiveOpportunityForVehicleAsync(long vehicleId, long? excludeOpportunityId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Set<SaleOpportunity>()
                .Where(so => so.VehicleId == vehicleId
                    && so.Status != SaleOpportunityStatus.Sold
                    && so.Status != SaleOpportunityStatus.Lost);

            if (excludeOpportunityId.HasValue)
            {
                query = query.Where(so => so.Id != excludeOpportunityId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<bool> HasAnyOpportunityForVehicleAsync(long vehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<SaleOpportunity>()
                .AnyAsync(so => so.VehicleId == vehicleId, cancellationToken);
        }
    }
}
