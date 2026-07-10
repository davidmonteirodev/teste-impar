using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.SaleOpportunities
{
    public sealed record SaleOpportunityResponse(
        long Id,
        DateTime CreateDate,
        DateTime? ModificationDate,
        long CustomerId,
        long VehicleId,
        SaleOpportunityStatus Status,
        decimal ProposedValue,
        string Notes);

    internal static class SaleOpportunityMappings
    {
        public static SaleOpportunityResponse ToResponse(this SaleOpportunity saleOpportunity)
        {
            return new SaleOpportunityResponse(
                saleOpportunity.Id,
                saleOpportunity.CreateDate,
                saleOpportunity.ModificationDate,
                saleOpportunity.CustomerId,
                saleOpportunity.VehicleId,
                saleOpportunity.Status,
                saleOpportunity.ProposedValue,
                saleOpportunity.Notes);
        }
    }
}
