using MediatR;

namespace VehicleCRM.Application.Features.Vehicles.Queries
{
    public sealed record GetVehicleByIdQuery(long Id) : IRequest<VehicleResponse?>;
}
