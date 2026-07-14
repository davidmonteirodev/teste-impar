using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.SaleOpportunities.Exceptions
{
    public class VehicleInActiveOpportunityException : DomainException
    {
        public VehicleInActiveOpportunityException(string vehicleModel)
            : base($"O veículo '{vehicleModel}' já está em outra oportunidade de venda ativa.")
        {
        }
    }
}
