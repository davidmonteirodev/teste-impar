using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Repositories.Criteria;

namespace VehicleCRM.Domain.SaleOpportunities.Repositories
{
    public interface ISaleOpportunityRepository : IBaseRepository<SaleOpportunity>
    {
        Task<(IEnumerable<SaleOpportunity> Items, int TotalCount)> GetPagedAsync(
            SaleOpportunitySearchCriteria criteria,
            CancellationToken cancellationToken = default);

        Task<SaleOpportunity> GetByIdWithRelationsAsync(long id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByCustomerAndVehicleAsync(long customerId, long vehicleId, CancellationToken cancellationToken = default);
    }
}
