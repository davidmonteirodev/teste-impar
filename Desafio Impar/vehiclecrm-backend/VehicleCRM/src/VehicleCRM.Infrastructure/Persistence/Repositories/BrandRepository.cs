using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Interfaces.Repositories;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        public BrandRepository(VehicleCrmDbContext context) : base(context)
        {
        }
    }
}
