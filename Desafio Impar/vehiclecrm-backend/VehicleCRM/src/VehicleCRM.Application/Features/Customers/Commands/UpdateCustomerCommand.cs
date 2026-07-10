using MediatR;
using VehicleCRM.Domain.Enums;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed record UpdateCustomerCommand(
        long Id,
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest) : IRequest;

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

            customer.Update(request.Name, request.Email, request.Phone, request.MainInterest);

            await _customerRepository.UpdateAsync(customer, cancellationToken);
        }
    }
}
