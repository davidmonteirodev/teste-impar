using MediatR;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record UpdateCustomerCommand(
        long Id,
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest) : IRequest;
}
