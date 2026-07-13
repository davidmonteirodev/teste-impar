using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Domain.Common.Exceptions;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class UpdateSaleOpportunityCommandHandler : BaseSaleOpportunityCommandHandler, IRequestHandler<UpdateSaleOpportunityCommand>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var saleOpportunity = await _saleOpportunityRepository.GetByIdAsync(request.Id, cancellationToken);

            if (saleOpportunity is null)
            {
                throw new EntityNotFoundException("Oportunidade de Venda", request.Id);
            }

            saleOpportunity.EnsureCanBeEdited(request.CustomerId, request.VehicleId);

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

            var vehicleChanged = saleOpportunity.VehicleId != request.VehicleId;

            if (vehicleChanged)
            {
                var hasActiveOpportunityForVehicle = await _saleOpportunityRepository.HasActiveOpportunityForVehicleAsync(
                    request.VehicleId, 
                    request.Id, 
                    cancellationToken);

                if (hasActiveOpportunityForVehicle)
                {
                    throw new DomainException("Já existe uma oportunidade ativa para este veículo.");
                }
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            if (vehicleChanged)
            {
                var previousVehicle = await _vehicleRepository.GetByIdAsync(saleOpportunity.VehicleId, cancellationToken);

                if (previousVehicle is not null)
                {
                    await SetVehicleAsAvailableAsync(previousVehicle, _vehicleRepository, cancellationToken);
                }
            }

            saleOpportunity.Update(
                customer.Id,
                vehicle.Id,
                request.Status,
                request.ProposedValue,
                request.Notes);

            await _saleOpportunityRepository.UpdateAsync(saleOpportunity, cancellationToken);

            await UpdateVehicleStatusBasedOnSaleOpportunityAsync(vehicle, request.Status, _vehicleRepository, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
    }
}
