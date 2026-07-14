namespace VehicleCRM.Domain.Vehicles.Repositories.Criteria
{
    public sealed class VehicleSearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? Color { get; set; }
        public int? MileageFrom { get; set; }
        public int? MileageTo { get; set; }
    }
}
