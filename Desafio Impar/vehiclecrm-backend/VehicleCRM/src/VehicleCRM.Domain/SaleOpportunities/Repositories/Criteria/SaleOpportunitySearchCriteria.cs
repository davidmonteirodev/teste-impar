using VehicleCRM.Domain.SaleOpportunities.Enums;

namespace VehicleCRM.Domain.SaleOpportunities.Repositories.Criteria
{
    public sealed class SaleOpportunitySearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? CustomerName { get; set; }
        public string? VehicleModel { get; set; }
        public SaleOpportunityStatus? Status { get; set; }
        public decimal? ProposedValueFrom { get; set; }
        public decimal? ProposedValueTo { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }
    }
}
