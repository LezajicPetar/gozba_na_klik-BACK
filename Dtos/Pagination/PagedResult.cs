namespace gozba_na_klik.Dtos.Pagination
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
