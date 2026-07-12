using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.SaleOpportunities.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Exceptions;

namespace VehicleCRM.Domain.SaleOpportunities.Services
{
    public class SaleOpportunityDomainService : ISaleOpportunityDomainService
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public SaleOpportunityDomainService(ISaleOpportunityRepository saleOpportunityRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task ValidateNewSaleOpportunityAsync(
            Customer customer,
            Vehicle vehicle,
            CancellationToken cancellationToken = default)
        {
            if (vehicle.Status != VehicleSaleStatus.Available)
            {
                throw new VehicleNotAvailableException(vehicle.Model);
            }

            var exists = await _saleOpportunityRepository.ExistsByCustomerAndVehicleAsync(
                customer.Id,
                vehicle.Id,
                cancellationToken);

            if (exists)
            {
                throw new DuplicateSaleOpportunityException(customer.Name, vehicle.Model);
            }
        }
    }
}
