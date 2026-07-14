using MediatR;
using VehicleCRM.Application.Common.Models;

namespace VehicleCRM.Application.Features.Vehicles.Queries
{
    public sealed record GetVehiclesQuery : PaginationRequest, IRequest<PagedResult<VehicleResponse>>
    {
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public int? YearFrom { get; init; }
        public int? YearTo { get; init; }
        public decimal? PriceFrom { get; init; }
        public decimal? PriceTo { get; init; }
        public string? Color { get; init; }
        public int? MileageFrom { get; init; }
        public int? MileageTo { get; init; }
    }
}
