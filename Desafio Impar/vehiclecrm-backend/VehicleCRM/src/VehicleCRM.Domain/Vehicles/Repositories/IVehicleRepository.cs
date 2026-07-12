using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Repositories.Criteria;

namespace VehicleCRM.Domain.Vehicles.Repositories
{
    public interface IVehicleRepository : IBaseRepository<Vehicle>
    {
        Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetPagedAsync(
            VehicleSearchCriteria criteria,
            CancellationToken cancellationToken = default);
    }
}
