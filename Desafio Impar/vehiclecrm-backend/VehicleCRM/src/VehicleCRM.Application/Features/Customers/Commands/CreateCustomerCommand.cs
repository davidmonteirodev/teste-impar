using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Enums;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record CreateCustomerCommand(
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest) : IRequest<EntityCreatedResponse>;

    public sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, EntityCreatedResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<EntityCreatedResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer(request.Name, request.Email, request.Phone, request.MainInterest);

            await _customerRepository.InsertAsync(customer, cancellationToken);

            return new EntityCreatedResponse(customer.Id, customer.CreateDate);
        }
    }
}
