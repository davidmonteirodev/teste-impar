using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
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

        Task<bool> HasActiveOpportunityForVehicleAsync(long vehicleId, long? excludeOpportunityId = null, CancellationToken cancellationToken = default);

        Task<bool> HasAnyOpportunityForVehicleAsync(long vehicleId, CancellationToken cancellationToken = default);

        Task<bool> HasAnyOpportunityForCustomerAsync(long customerId, CancellationToken cancellationToken = default);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<Dictionary<SaleOpportunityStatus, int>> GetCountByStatusAsync(CancellationToken cancellationToken = default);
    }
}
