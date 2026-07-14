namespace VehicleCRM.Application.Common.Models
{
    public abstract record PaginationRequest
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 100;

        private int _page = DefaultPage;
        private int _pageSize = DefaultPageSize;

        public int Page
        {
            get => _page;
            init => _page = value < 1 ? DefaultPage : value;
        }

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value switch
            {
                < 1 => DefaultPageSize,
                > MaxPageSize => MaxPageSize,
                _ => value
            };
        }
    }
}
