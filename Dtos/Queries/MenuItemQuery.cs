using gozba_na_klik.Dtos.Pagination;

namespace gozba_na_klik.Dtos.Queries
{
    public class MenuItemQuery : PagedRequest
    {
        public string? Name { get; set; }
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
    }
}
