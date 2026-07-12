using MediatR;
using VehicleCRM.Application.Common.Exceptions;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken);

            if (vehicle is null)
            {
                throw new EntityNotFoundException("Veículo", request.Id);
            }

            vehicle.Update(
                request.Brand,
                request.Model,
                request.Year,
                request.Price,
                request.Color,
                request.Mileage,
                request.Status);

            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        }
    }
}
