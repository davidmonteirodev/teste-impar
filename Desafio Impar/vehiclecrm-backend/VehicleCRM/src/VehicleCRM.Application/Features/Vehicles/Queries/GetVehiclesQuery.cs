using MediatR;
using VehicleCRM.Domain.Interfaces.Repositories;

namespace VehicleCRM.Application.Features.Vehicles.Queries
{
    public sealed record GetVehiclesQuery : IRequest<IReadOnlyCollection<VehicleResponse>>;

    public sealed class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, IReadOnlyCollection<VehicleResponse>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<IReadOnlyCollection<VehicleResponse>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleRepository.GetAllAsync(cancellationToken);

            return vehicles
                .Select(vehicle => vehicle.ToResponse())
                .ToArray();
        }
    }
}
