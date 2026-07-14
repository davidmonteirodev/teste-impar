using MediatR;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed record DeleteSaleOpportunityCommand(long Id) : IRequest;
}
