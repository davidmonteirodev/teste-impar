using MediatR;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed record GetSaleOpportunitiesQuery : IRequest<IReadOnlyCollection<SaleOpportunityResponse>>;
}
