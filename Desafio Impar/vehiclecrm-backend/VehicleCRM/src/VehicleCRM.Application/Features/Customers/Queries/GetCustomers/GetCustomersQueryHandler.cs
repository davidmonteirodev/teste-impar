using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.Customers.Repositories.Criteria;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, PagedResult<CustomerResponse>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<PagedResult<CustomerResponse>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var criteria = new CustomerSearchCriteria
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                MainInterest = request.MainInterest
            };

            var (customers, totalCount) = await _customerRepository.GetPagedAsync(criteria, cancellationToken);

            var customerResponses = customers
                .Select(customer => customer.ToResponse())
                .ToArray();

            return new PagedResult<CustomerResponse>(
                customerResponses,
                totalCount,
                request.Page,
                request.PageSize);
        }
    }
}
