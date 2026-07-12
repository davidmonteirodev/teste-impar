using MediatR;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            await _vehicleRepository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
