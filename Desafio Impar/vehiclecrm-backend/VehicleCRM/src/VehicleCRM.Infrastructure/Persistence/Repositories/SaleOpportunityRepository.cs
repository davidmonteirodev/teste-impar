using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Interfaces.Repositories;
using VehicleCRM.Infrastructure.Persistence.Contexts;
using VehicleCRM.Infrastructure.Persistence.Repositories.Base;

namespace VehicleCRM.Infrastructure.Persistence.Repositories
{
    public class SaleOpportunityRepository : BaseRepository<SaleOpportunity>, ISaleOpportunityRepository
    {
        public SaleOpportunityRepository(VehicleCrmDbContext context) : base(context)
        {
        }
    }
}
