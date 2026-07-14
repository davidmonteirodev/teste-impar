using MediatR;

namespace VehicleCRM.Application.Features.SaleOpportunities.Queries
{
    public sealed record GetSaleOpportunityByIdQuery(long Id) : IRequest<SaleOpportunityResponse?>;
}
