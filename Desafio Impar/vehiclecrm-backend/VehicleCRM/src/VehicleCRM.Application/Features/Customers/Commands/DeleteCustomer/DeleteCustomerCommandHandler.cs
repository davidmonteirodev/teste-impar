using MediatR;
using VehicleCRM.Domain.Customers.Exceptions;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;

namespace VehicleCRM.Application.Features.Customers.Commands
{
    public sealed class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public DeleteCustomerCommandHandler(
            ICustomerRepository customerRepository,
            ISaleOpportunityRepository saleOpportunityRepository)
        {
            _customerRepository = customerRepository;
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var hasOpportunities = await _saleOpportunityRepository.HasAnyOpportunityForCustomerAsync(
                request.Id,
                cancellationToken);

            if (hasOpportunities)
            {
                throw new CustomerHasSaleOpportunitiesException();
            }

            await _customerRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
