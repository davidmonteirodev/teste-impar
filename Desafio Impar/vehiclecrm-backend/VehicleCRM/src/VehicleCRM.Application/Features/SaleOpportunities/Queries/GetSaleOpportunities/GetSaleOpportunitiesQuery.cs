using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.SaleOpportunities.Enums;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed record GetSaleOpportunitiesQuery : PaginationRequest, IRequest<PagedResult<SaleOpportunityResponse>>
    {
        public string? CustomerName { get; init; }
        public string? VehicleModel { get; init; }
        public SaleOpportunityStatus? Status { get; init; }
        public decimal? ProposedValueFrom { get; init; }
        public decimal? ProposedValueTo { get; init; }
        public DateTime? CreateDateFrom { get; init; }
        public DateTime? CreateDateTo { get; init; }
    }
}
