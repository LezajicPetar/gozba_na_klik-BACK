namespace gozba_na_klik.Dtos.Pagination
{
    public class PagedRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }
        public string? SortDir { get; set; } = "asc";
    }
}
