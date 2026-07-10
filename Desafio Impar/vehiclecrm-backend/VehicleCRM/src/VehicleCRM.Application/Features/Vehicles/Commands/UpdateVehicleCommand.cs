using MediatR;
using VehicleCRM.Domain.Enums;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed record UpdateVehicleCommand(
        long Id,
        string Brand,
        string Model,
        int Year,
        decimal Price,
        string Color,
        int Mileage,
        VehicleSaleStatus Status) : IRequest;

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
