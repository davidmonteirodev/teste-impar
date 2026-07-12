using VehicleCRM.Domain.SaleOpportunities.Entities;
using VehicleCRM.Domain.SaleOpportunities.Repositories;
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
