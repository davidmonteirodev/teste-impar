using MediatR;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed record GetCustomersQuery : IRequest<IReadOnlyCollection<CustomerResponse>>;
}
