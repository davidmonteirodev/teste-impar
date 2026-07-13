using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, EntityCreatedResponse>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<EntityCreatedResponse> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = new Vehicle(
                request.Brand,
                request.Model,
                request.Year,
                request.Price,
                request.Color,
                request.Mileage);

            await _vehicleRepository.InsertAsync(vehicle, cancellationToken);

            return new EntityCreatedResponse(vehicle.Id, vehicle.CreateDate);
        }
    }
}
