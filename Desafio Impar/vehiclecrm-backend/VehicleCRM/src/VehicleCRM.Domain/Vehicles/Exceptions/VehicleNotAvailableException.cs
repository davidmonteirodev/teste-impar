using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Vehicles.Exceptions
{
    public class VehicleNotAvailableException : DomainException
    {
        public VehicleNotAvailableException(string vehicleModel)
            : base($"O veículo {vehicleModel} não está disponível para venda.")
        {
        }
    }
}
