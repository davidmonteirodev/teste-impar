namespace VehicleCRM.Application.Common.Models
{
    public sealed record PagedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; init; }
        public int TotalItems { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 0;
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PagedResult(IReadOnlyCollection<T> items, int totalItems, int page, int pageSize)
        {
            Items = items ?? Array.Empty<T>();
            TotalItems = totalItems;
            Page = page;
            PageSize = pageSize;
        }
    }
}
