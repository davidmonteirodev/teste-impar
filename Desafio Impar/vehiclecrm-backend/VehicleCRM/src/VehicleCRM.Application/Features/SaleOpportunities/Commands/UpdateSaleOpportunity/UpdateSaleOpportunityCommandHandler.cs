using MediatR;
using VehicleCRM.Application.Common.Exceptions;
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

            if (saleOpportunity is null)
            {
                throw new EntityNotFoundException("Oportunidade de Venda", request.Id);
            }

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new EntityNotFoundException("Cliente", request.CustomerId);
            }

            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);

            if (vehicle is null)
            {
                throw new EntityNotFoundException("Veículo", request.VehicleId);
            }

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
