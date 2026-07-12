using MediatR;
using VehicleCRM.Domain.Customers.Repositories;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IReadOnlyCollection<CustomerResponse>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IReadOnlyCollection<CustomerResponse>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync(cancellationToken);

            return customers
                .Select(customer => customer.ToResponse())
                .ToArray();
        }
    }
}
