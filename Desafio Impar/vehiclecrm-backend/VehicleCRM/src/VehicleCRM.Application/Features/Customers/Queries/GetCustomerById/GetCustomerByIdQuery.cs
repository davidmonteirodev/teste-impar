using MediatR;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed record GetCustomerByIdQuery(long Id) : IRequest<CustomerResponse?>;
}
