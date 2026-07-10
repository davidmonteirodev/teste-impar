using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.Vehicles
{
    public sealed record VehicleResponse(
        long Id,
        DateTime CreateDate,
        DateTime? ModificationDate,
        string Brand,
        string Model,
        int Year,
        decimal Price,
        string Color,
        int Mileage,
        VehicleSaleStatus Status);

    internal static class VehicleMappings
    {
        public static VehicleResponse ToResponse(this Vehicle vehicle)
        {
            return new VehicleResponse(
                vehicle.Id,
                vehicle.CreateDate,
                vehicle.ModificationDate,
                vehicle.Brand,
                vehicle.Model,
                vehicle.Year,
                vehicle.Price,
                vehicle.Color,
                vehicle.Mileage,
                vehicle.Status);
        }
    }
}
