using MediatR;
using VehicleCRM.Application.Common.Models;

namespace VehicleCRM.Application.Features.Vehicles.Commands
{
    public sealed record CreateVehicleCommand(
        string? Brand,
        string? Model,
        int Year,
        decimal Price,
        string? Color,
        int Mileage) : IRequest<EntityCreatedResponse>;
}
