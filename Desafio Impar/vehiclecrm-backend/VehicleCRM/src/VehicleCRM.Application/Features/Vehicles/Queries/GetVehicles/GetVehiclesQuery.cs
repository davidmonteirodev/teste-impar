using MediatR;

namespace VehicleCRM.Application.Features.Vehicles.Queries
{
    public sealed record GetVehiclesQuery : IRequest<IReadOnlyCollection<VehicleResponse>>;
}
