using VehicleCRM.Domain.Entities.Base;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Domain.Entities
{
    public class SaleOpportunity : BaseEntity
    {
        protected SaleOpportunity() { }

        public SaleOpportunity(long customerId, long vehicleId, SaleOpportunityStatus status, decimal proposedValue, string negotiationNotes)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
            Status = status;
            ProposedValue = proposedValue;
            NegotiationNotes = negotiationNotes;
        }

        public virtual long CustomerId { get; private set; }
        public virtual long VehicleId { get; private set; }
        public virtual SaleOpportunityStatus Status { get; private set; }
        public virtual decimal ProposedValue { get; private set; }
        public virtual string NegotiationNotes { get; private set; }

        public virtual Customer Customer { get; private set; }
        public virtual Vehicle Vehicle { get; private set; }
    }
}
