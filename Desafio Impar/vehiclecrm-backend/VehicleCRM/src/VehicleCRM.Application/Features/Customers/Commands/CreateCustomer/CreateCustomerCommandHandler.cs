using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Exceptions;
using VehicleCRM.Domain.Customers.Repositories;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, EntityCreatedResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<EntityCreatedResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _customerRepository.ExistsByEmailAsync(request.Email, cancellationToken);

            if (emailExists)
            {
                throw new DuplicateCustomerEmailException(request.Email);
            }

            var customer = Customer.Create(request.Name, request.Email, request.Phone, request.MainInterest);

           await _customerRepository.InsertAsync(customer, cancellationToken);

            return new EntityCreatedResponse(customer.Id, customer.CreateDate);
        }
    }
}
