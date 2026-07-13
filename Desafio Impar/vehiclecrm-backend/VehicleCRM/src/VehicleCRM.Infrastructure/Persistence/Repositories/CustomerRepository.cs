using Microsoft.EntityFrameworkCore;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Repositories;
using VehicleCRM.Domain.Customers.Repositories.Criteria;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(VehicleCrmDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
            CustomerSearchCriteria criteria,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Set<Customer>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(criteria.Name))
                query = query.Where(c => EF.Functions.Like(c.Name, $"%{criteria.Name}%"));

            if (!string.IsNullOrWhiteSpace(criteria.Email))
                query = query.Where(c => EF.Functions.Like(c.Email, $"%{criteria.Email}%"));

            if (!string.IsNullOrWhiteSpace(criteria.Phone))
                query = query.Where(c => EF.Functions.Like(c.Phone, $"%{criteria.Phone}%"));

            if (criteria.MainInterest.HasValue)
                query = query.Where(c => c.MainInterest == criteria.MainInterest.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.Id)
                .Skip((criteria.Page - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Customer>()
                .AsNoTracking()
                .AnyAsync(c => c.Email == email, cancellationToken);
        }
    }
}
