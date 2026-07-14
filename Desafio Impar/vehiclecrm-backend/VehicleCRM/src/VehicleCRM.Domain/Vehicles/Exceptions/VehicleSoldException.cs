using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Vehicles.Exceptions
{
    public class VehicleSoldException : DomainException
    {
        public VehicleSoldException()
            : base("Não é possível editar um veículo vendido.")
        {
        }
    }
}
