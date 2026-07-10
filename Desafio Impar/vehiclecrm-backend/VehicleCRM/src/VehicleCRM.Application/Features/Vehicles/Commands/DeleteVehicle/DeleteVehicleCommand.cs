using MediatR;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed record DeleteVehicleCommand(long Id) : IRequest;
}
