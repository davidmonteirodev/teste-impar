using MediatR;
using VehicleCRM.Application.Common.Models;
using VehicleCRM.Domain.Vehicles.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories.Criteria;

namespace VehicleCRM.Application.Features.Vehicles.Queries
{
    public sealed class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, PagedResult<VehicleResponse>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        public GetVehiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<PagedResult<VehicleResponse>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            var criteria = new VehicleSearchCriteria
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Brand = request.Brand,
                Model = request.Model,
                YearFrom = request.YearFrom,
                YearTo = request.YearTo,
                PriceFrom = request.PriceFrom,
                PriceTo = request.PriceTo,
                Color = request.Color,
                MileageFrom = request.MileageFrom,
                MileageTo = request.MileageTo
            };

            var (vehicles, totalCount) = await _vehicleRepository.GetPagedAsync(criteria, cancellationToken);

            var vehicleResponses = vehicles
                .Select(vehicle => vehicle.ToResponse())
                .ToArray();

            return new PagedResult<VehicleResponse>(
                vehicleResponses,
                totalCount,
                request.Page,
                request.PageSize);
        }
    }
}
