using VehicleCRM.Domain.Common.Repositories;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Repositories.Criteria;

namespace VehicleCRM.Domain.Customers.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<(IEnumerable<Customer> Items, int TotalCount)> GetPagedAsync(
            CustomerSearchCriteria criteria,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
    }
}
