using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed record CreateSaleOpportunityCommand(
        long CustomerId,
        long VehicleId,
        SaleOpportunityStatus Status,
        decimal ProposedValue,
        string Notes) : IRequest<EntityCreatedResponse>;
}
