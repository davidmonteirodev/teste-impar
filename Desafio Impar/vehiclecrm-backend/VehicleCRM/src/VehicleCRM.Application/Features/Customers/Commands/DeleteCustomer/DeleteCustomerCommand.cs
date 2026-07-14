using MediatR;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record DeleteCustomerCommand(long Id) : IRequest;
}
