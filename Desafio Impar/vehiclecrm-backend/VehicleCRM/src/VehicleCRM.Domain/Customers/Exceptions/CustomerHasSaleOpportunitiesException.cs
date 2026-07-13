using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Customers.Exceptions
{
    public class CustomerHasSaleOpportunitiesException : DomainException
    {
        public CustomerHasSaleOpportunitiesException()
            : base("Não é possível excluir um cliente que possui oportunidades de venda associadas.")
        {
        }
    }
}
