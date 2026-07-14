using VehicleCRM.Domain.Customers.Entities;
using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Application.Features.Customers
{
    public sealed record CustomerResponse(
        long Id,
        DateTime CreateDate,
        DateTime? ModificationDate,
        string Name,
        string Email,
        string Phone,
        CustomerMainInterest MainInterest);

    internal static class CustomerMappings
    {
        public static CustomerResponse ToResponse(this Customer customer)
        {
            return new CustomerResponse(
                customer.Id,
                customer.CreateDate,
                customer.ModificationDate,
                customer.Name,
                customer.Email,
                customer.Phone,
                customer.MainInterest);
        }
    }
}
