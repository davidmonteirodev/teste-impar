using MediatR;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.Customers.Queries
{
    public sealed record GetCustomerByIdQuery(long Id) : IRequest<CustomerResponse>;

    public sealed class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            return customer.ToResponse();
        }
    }
}
