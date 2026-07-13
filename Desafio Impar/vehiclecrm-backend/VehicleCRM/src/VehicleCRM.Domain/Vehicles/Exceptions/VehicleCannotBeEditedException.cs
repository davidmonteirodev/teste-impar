using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Vehicles.Exceptions
{
    public class VehicleCannotBeEditedException : DomainException
    {
        public VehicleCannotBeEditedException(string status)
            : base($"Não é possível editar um veículo com status '{status}'.")
        {
        }
    }
}
