using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;

namespace VehicleCRM.Application.Features.SaleOpportunities
{
    public sealed record VehicleBasicInfo(
        long VehicleId,
        string Model);

    public sealed record CustomerBasicInfo(
        long CustomerId,
        string Name);

    public sealed record SaleOpportunityResponse(
        long Id,
        DateTime CreateDate,
        DateTime? ModificationDate,
        SaleOpportunityStatus Status,
        decimal ProposedValue,
        string Notes,
        VehicleBasicInfo Vehicle,
        CustomerBasicInfo Customer);

    internal static class SaleOpportunityMappings
    {
        public static SaleOpportunityResponse ToResponse(this SaleOpportunity saleOpportunity)
        {
            var vehicle = new VehicleBasicInfo(
                saleOpportunity.VehicleId, 
                saleOpportunity.Vehicle.Model);

            var customer = new CustomerBasicInfo(
                saleOpportunity.CustomerId, 
                saleOpportunity.Customer.Name);

            return new SaleOpportunityResponse(
                saleOpportunity.Id,
                saleOpportunity.CreateDate,
                saleOpportunity.ModificationDate,
                saleOpportunity.Status,
                saleOpportunity.ProposedValue,
                saleOpportunity.Notes,
                vehicle,
                customer);
        }
    }
}