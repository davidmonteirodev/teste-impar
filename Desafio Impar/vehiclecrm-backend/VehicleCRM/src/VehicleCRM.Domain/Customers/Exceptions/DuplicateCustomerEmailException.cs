using VehicleCRM.Domain.Common.Exceptions;

namespace VehicleCRM.Domain.Customers.Exceptions
{
    public class DuplicateCustomerEmailException : DomainException
    {
        public DuplicateCustomerEmailException(string email)
            : base($"Já existe um cliente cadastrado com o email '{email}'.")
        {
        }
    }
}
