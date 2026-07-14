using MediatR;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
using VehicleCRM.Domain.Vehicles.Exceptions;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ISaleOpportunityRepository _saleOpportunityRepository;

        public DeleteVehicleCommandHandler(
            IVehicleRepository vehicleRepository,
            ISaleOpportunityRepository saleOpportunityRepository)
        {
            _vehicleRepository = vehicleRepository;
            _saleOpportunityRepository = saleOpportunityRepository;
        }

        public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            var hasOpportunities = await _saleOpportunityRepository
                .HasAnyOpportunityForVehicleAsync(request.Id, cancellationToken);

            if (hasOpportunities)
            {
                throw new VehicleHasSaleOpportunitiesException();
            }

            await _vehicleRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
