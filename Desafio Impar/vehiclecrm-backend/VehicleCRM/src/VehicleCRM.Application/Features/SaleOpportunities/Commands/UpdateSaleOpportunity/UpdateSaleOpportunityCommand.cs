using MediatR;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed record UpdateSaleOpportunityCommand(
        long Id,
        long CustomerId,
        long VehicleId,
        SaleOpportunityStatus Status,
        decimal ProposedValue,
        string Notes) : IRequest;
}
