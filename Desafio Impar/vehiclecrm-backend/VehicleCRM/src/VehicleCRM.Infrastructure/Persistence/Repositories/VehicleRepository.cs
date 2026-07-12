using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Vehicles.Entities;
using VehicleCRM.Domain.Vehicles.Repositories;
using VehicleCRM.Domain.Vehicles.Repositories.Criteria;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(VehicleCrmDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Vehicle> Items, int TotalCount)> GetPagedAsync(
            VehicleSearchCriteria criteria,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<Vehicle>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Brand))
                query = query.Where(v => EF.Functions.Like(v.Brand, $"%{criteria.Brand}%"));

            if (!string.IsNullOrWhiteSpace(criteria.Model))
                query = query.Where(v => EF.Functions.Like(v.Model, $"%{criteria.Model}%"));

            if (criteria.YearFrom.HasValue)
                query = query.Where(v => v.Year >= criteria.YearFrom.Value);

            if (criteria.YearTo.HasValue)
                query = query.Where(v => v.Year <= criteria.YearTo.Value);

            if (criteria.PriceFrom.HasValue)
                query = query.Where(v => v.Price >= criteria.PriceFrom.Value);

            if (criteria.PriceTo.HasValue)
                query = query.Where(v => v.Price <= criteria.PriceTo.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Color))
                query = query.Where(v => EF.Functions.Like(v.Color, $"%{criteria.Color}%"));

            if (criteria.MileageFrom.HasValue)
                query = query.Where(v => v.Mileage >= criteria.MileageFrom.Value);

            if (criteria.MileageTo.HasValue)
                query = query.Where(v => v.Mileage <= criteria.MileageTo.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(v => v.Id)
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
