using VehicleCRM.Domain.Common.Entities;
using VehicleCRM.Domain.Common.Exceptions;
using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.SaleOpportunities.Enums;
using VehicleCRM.Domain.Vehicles.Entities;

namespace VehicleCRM.Domain.SaleOpportunities.Entities
{
    public class SaleOpportunity : BaseEntity
    {
        protected SaleOpportunity() { }

        private SaleOpportunity(long customerId, long vehicleId, SaleOpportunityStatus status, decimal proposedValue, string? notes)
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
        public virtual string? Notes { get; private set; }

        public virtual Customer Customer { get; private set; }
        public virtual Vehicle Vehicle { get; private set; }

        public static SaleOpportunity Create(long customerId, long vehicleId, decimal proposedValue, string? notes)
        {
            return new SaleOpportunity(customerId, vehicleId, SaleOpportunityStatus.NewLead, proposedValue, notes);
        }

        public virtual bool IsFinalized() => Status == SaleOpportunityStatus.Sold || Status == SaleOpportunityStatus.Lost;

        public virtual bool IsInNegotiation() => Status == SaleOpportunityStatus.InNegotiation || Status == SaleOpportunityStatus.ProposalSent;

        public virtual void EnsureCanBeEdited(long newCustomerId, long newVehicleId, SaleOpportunityStatus newStatus)
        {
            EnsureStatusTransitionIsValid(newStatus);

            if (IsFinalized())
            {
                throw new DomainException("Esta oportunidade está finalizada e não pode mais ser editada.");
            }

            EnsureVehicleCanBeChanged(newVehicleId);
            EnsureCustomerCanBeChanged(newCustomerId);
        }

        public virtual void Update(long customerId, long vehicleId, SaleOpportunityStatus status, decimal proposedValue, string? notes)
        {
            CustomerId = customerId;
            VehicleId = vehicleId;
            Status = status;
            ProposedValue = proposedValue;
            Notes = notes;
        }

        private void EnsureVehicleCanBeChanged(long newVehicleId)
        {
            if (VehicleId != newVehicleId && IsInNegotiation())
            {
                throw new DomainException("Não é permitido trocar o veículo quando a oportunidade está em Negociação ou com Proposta Enviada.");
            }
        }

        private void EnsureCustomerCanBeChanged(long newCustomerId)
        {
            if (CustomerId != newCustomerId && IsInNegotiation())
            {
                throw new DomainException("Não é permitido trocar o cliente quando a oportunidade está em Negociação ou com Proposta Enviada.");
            }
        }

        private void EnsureStatusTransitionIsValid(SaleOpportunityStatus newStatus)
        {
            if (Status == SaleOpportunityStatus.InNegotiation && newStatus == SaleOpportunityStatus.NewLead)
            {
                throw new DomainException("Não é permitido alterar o status de 'Em negociação' para 'Novo lead'.");
            }

            if (Status == SaleOpportunityStatus.ProposalSent && 
                (newStatus == SaleOpportunityStatus.NewLead || newStatus == SaleOpportunityStatus.InNegotiation))
            {
                throw new DomainException("Não é permitido alterar o status de 'Proposta enviada' para 'Novo lead' ou 'Em negociação'.");
            }
        }
    }
}
