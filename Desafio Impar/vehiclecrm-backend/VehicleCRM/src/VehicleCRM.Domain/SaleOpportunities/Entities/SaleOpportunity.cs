using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.Vehicles.Entities;

namespace VehicleCRM.Domain.SaleOpportunities.Entities
{
    public class SaleOpportunity : BaseEntity
    {
        protected SaleOpportunity() { }

        public SaleOpportunity(long customerId, long vehicleId, SaleOpportunityStatus status, decimal proposedValue, string notes)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
            Status = status;
            ProposedValue = proposedValue;
            Notes = notes;
        }

        public virtual long CustomerId { get; private set; }
        public virtual long VehicleId { get; private set; }
        public virtual SaleOpportunityStatus Status { get; private set; }
        public virtual decimal ProposedValue { get; private set; }
        public virtual string Notes { get; private set; }

        public virtual Customer Customer { get; private set; }
        public virtual Vehicle Vehicle { get; private set; }

        public virtual void Update(long customerId, long vehicleId, SaleOpportunityStatus status, decimal proposedValue, string notes)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
            Status = status;
            ProposedValue = proposedValue;
            Notes = notes;
        }
    }
}
