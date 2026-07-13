using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.SaleOpportunities.Exceptions
{
    public class CannotDeleteSoldOpportunityException : DomainException
    {
        public CannotDeleteSoldOpportunityException()
            : base("Não é permitido excluir uma oportunidade de venda com status 'Vendido'.")
        {
        }
    }
}
