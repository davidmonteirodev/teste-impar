using MediatR;
using VehicleCRM.Domain.Enums;

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
}
