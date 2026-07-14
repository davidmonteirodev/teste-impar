using MediatR;
using VehicleCRM.Application.Common.Models;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed record CreateSaleOpportunityCommand(
        long CustomerId,
        long VehicleId,
        decimal ProposedValue,
        string? Notes) : IRequest<EntityCreatedResponse>;
}
