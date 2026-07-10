using VehicleCRM.Domain.Entities;
using VehicleCRM.Domain.Enums;

namespace VehicleCRM.Application.Features.Customers
{
    public sealed record CustomerResponse(
        long Id,
        DateTime CreateDate,
        DateTime? ModificationDate,
        DateTime? DeleteDate,
        bool IsDeleted,
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
                customer.DeleteDate,
                customer.IsDeleted,
                customer.Name,
                customer.Email,
                customer.Phone,
                customer.MainInterest);
        }
    }
}
