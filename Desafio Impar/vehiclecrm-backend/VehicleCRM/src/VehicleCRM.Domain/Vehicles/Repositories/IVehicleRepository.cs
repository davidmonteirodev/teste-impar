using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Enums;
using VehicleCRM.Domain.Vehicles.Repositories.Criteria;

namespace VehicleCRM.Domain.Vehicles.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetPagedAsync(
            VehicleSearchCriteria criteria,
            CancellationToken cancellationToken = default);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<Dictionary<VehicleSaleStatus, int>> GetCountByStatusAsync(CancellationToken cancellationToken = default);

        Task<decimal> GetTotalSoldVehiclesValueAsync(CancellationToken cancellationToken = default);
    }
}
