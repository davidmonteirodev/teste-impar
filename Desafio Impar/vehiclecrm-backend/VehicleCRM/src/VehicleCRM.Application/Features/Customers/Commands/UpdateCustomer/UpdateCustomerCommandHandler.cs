using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Domain.Customers.Repositories;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);

            if (customer is null)
            {
                throw new EntityNotFoundException("Cliente", request.Id);
            }

            customer.Update(request.Name, request.Phone, request.MainInterest);

            await _customerRepository.UpdateAsync(customer, cancellationToken);
        }
    }
}
