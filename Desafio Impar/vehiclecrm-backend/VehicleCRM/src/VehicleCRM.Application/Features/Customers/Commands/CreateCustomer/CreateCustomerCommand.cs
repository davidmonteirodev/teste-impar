using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record CreateCustomerCommand(
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest) : IRequest<EntityCreatedResponse>;
}
