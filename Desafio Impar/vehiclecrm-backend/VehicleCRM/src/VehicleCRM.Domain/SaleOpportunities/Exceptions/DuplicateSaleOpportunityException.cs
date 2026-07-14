using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.SaleOpportunities.Exceptions
{
    public class DuplicateSaleOpportunityException : DomainException
    {
        public DuplicateSaleOpportunityException(string custumerName, string vehicleModel)
            : base($"Já existe uma oportunidade de venda para o cliente '{custumerName}' e veículo '{vehicleModel}'.")
        {
        }
    }
}
