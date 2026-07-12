using MediatR;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class UpdateSaleOpportunityCommandHandler : IRequestHandler<UpdateSaleOpportunityCommand>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public UpdateSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(UpdateSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var saleOpportunity = await _saleOpportunityRepository.GetByIdAsync(request.Id, cancellationToken);
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            saleOpportunity.Update(
                customer.Id,
                vehicle.Id,
                request.Status,
                request.ProposedValue,
                request.Notes);

            await _saleOpportunityRepository.UpdateAsync(saleOpportunity, cancellationToken);
        }
    }
}
