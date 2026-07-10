using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Interfaces.Repositories;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(VehicleCrmDbContext context) : base(context)
        {
        }
    }
}
