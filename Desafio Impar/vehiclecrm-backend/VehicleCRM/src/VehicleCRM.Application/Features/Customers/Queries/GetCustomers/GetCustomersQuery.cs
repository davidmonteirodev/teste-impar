using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed record GetCustomersQuery : PaginationRequest, IRequest<PagedResult<CustomerResponse>>
    {
        public string? Name { get; init; }
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public CustomerMainInterest? MainInterest { get; init; }
    }
}
