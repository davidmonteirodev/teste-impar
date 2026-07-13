using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Domain.Common.UnitOfWork;
using VehicleCRM.Domain.SaleOpportunities.Exceptions;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.SaleOpportunities.Commands
{
    public sealed class DeleteSaleOpportunityCommandHandler : BaseSaleOpportunityCommandHandler, IRequestHandler<DeleteSaleOpportunityCommand>
    {
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSaleOpportunityCommandHandler(
            ISaleOpportunityRepository saleOpportunityRepository,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork)
            : base(vehicleRepository)
        {
            _saleOpportunityRepository = saleOpportunityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteSaleOpportunityCommand request, CancellationToken cancellationToken)
        {
            var saleOpportunity = await _saleOpportunityRepository.GetByIdAsync(request.Id, cancellationToken);

            if (saleOpportunity is null)
                throw new EntityNotFoundException("Oportunidade de venda", request.Id);

            if (saleOpportunity.IsFinalized())
                throw new CannotDeleteFinalizedOpportunityException();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var vehicle = await _vehicleRepository.GetByIdAsync(saleOpportunity.VehicleId, cancellationToken);

            if (vehicle is not null)
                await SetVehicleAsAvailableAsync(vehicle, cancellationToken);

            await _saleOpportunityRepository.DeleteAsync(request.Id, cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
    }
}