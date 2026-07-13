using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public abstract class BaseSaleOpportunityCommandHandler
    {
        protected static async Task UpdateVehicleStatusBasedOnSaleOpportunityAsync(
            Vehicle vehicle, 
            SaleOpportunityStatus saleOpportunityStatus, 
            IVehicleRepository vehicleRepository, 
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
                await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            }
        }

        protected static async Task SetVehicleAsAvailableAsync(
            Vehicle vehicle,
            IVehicleRepository vehicleRepository,
            CancellationToken cancellationToken)
        {
            if (vehicle.Status != VehicleSaleStatus.Available)
            {
                vehicle.UpdateStatus(VehicleSaleStatus.Available);
                await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            }
        }
    }
}
