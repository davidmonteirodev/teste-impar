using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Vehicles.Exceptions
{
    public class VehicleHasSaleOpportunitiesException : DomainException
    {
        public VehicleHasSaleOpportunitiesException()
            : base("Não é possível excluir um veículo que possui oportunidades de venda associadas.")
        {
        }
    }
}
