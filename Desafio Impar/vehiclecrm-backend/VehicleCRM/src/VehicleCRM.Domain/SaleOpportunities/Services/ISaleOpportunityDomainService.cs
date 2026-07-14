using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Vehicles.Entities;

namespace VehicleCRM.Domain.SaleOpportunities.Services
{
    public interface ISaleOpportunityDomainService
    {
        Task ValidateNewSaleOpportunityAsync(
            Customer customer,
            Vehicle vehicle,
            CancellationToken cancellationToken = default);
    }
}
