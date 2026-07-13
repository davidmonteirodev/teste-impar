using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.SaleOpportunities.Exceptions
{
    public class CannotDeleteFinalizedOpportunityException : DomainException
    {
        public CannotDeleteFinalizedOpportunityException()
            : base("Não é permitido excluir uma oportunidade de venda finalizada (com status 'Vendido' ou 'Perdido').")
        {
        }
    }
}
