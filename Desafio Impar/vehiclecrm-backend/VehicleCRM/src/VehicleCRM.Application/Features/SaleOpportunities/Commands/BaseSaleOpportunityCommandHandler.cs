using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public abstract class BaseSaleOpportunityCommandHandler
    {
        protected readonly IVehicleRepository _vehicleRepository;

        protected BaseSaleOpportunityCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        protected async Task UpdateVehicleStatusBasedOnSaleOpportunityAsync(
            Vehicle vehicle, 
            SaleOpportunityStatus saleOpportunityStatus, 
            CancellationToken cancellationToken)
        {
            var vehicleStatus = saleOpportunityStatus switch
            {
                SaleOpportunityStatus.InNegotiation => VehicleSaleStatus.Reserved,
                SaleOpportunityStatus.ProposalSent => VehicleSaleStatus.Reserved,
                SaleOpportunityStatus.Sold => VehicleSaleStatus.Sold,
                SaleOpportunityStatus.Lost => VehicleSaleStatus.Available,
                _ => vehicle.Status
            };

            if (vehicleStatus != vehicle.Status)
            {
                vehicle.UpdateStatus(vehicleStatus);
                await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            }
        }

        protected async Task SetVehicleAsAvailableAsync(
            Vehicle vehicle,
            CancellationToken cancellationToken)
        {
            if (vehicle.Status != VehicleSaleStatus.Available)
            {
                vehicle.UpdateStatus(VehicleSaleStatus.Available);
                await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            }
        }
    }
}
