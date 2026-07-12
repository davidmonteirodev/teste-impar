using VehicleCRM.Domain.Customers.Enums;

namespace VehicleCRM.Domain.Customers.Repositories.Criteria
{
    public sealed class CustomerSearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CustomerMainInterest? MainInterest { get; set; }
    }
}
