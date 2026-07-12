using MediatR;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record UpdateCustomerCommand(
        long Id,
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest) : IRequest;
}
