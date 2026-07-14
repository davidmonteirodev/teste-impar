namespace VehicleCRM.Application.Features.Dashboard
{
    public sealed record DashboardResponse
    {
        public required DashboardCardsResponse Cards { get; init; }
        public required IEnumerable<StatusCountResponse> VehicleStatus { get; init; }
        public required IEnumerable<StatusCountResponse> OpportunityStatus { get; init; }
    }

    public sealed record DashboardCardsResponse
    {
        public required int Vehicles { get; init; }
        public required int Customers { get; init; }
        public required int Opportunities { get; init; }
        public required decimal SoldVehiclesTotalValue { get; init; }
    }

    public sealed record StatusCountResponse
    {
        public required string Status { get; init; }
        public required int Count { get; init; }
    }
}
