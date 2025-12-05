using gozba_na_klik.Dtos.Pagination;

namespace gozba_na_klik.Dtos.Queries
{
    public class RestaurantQuery : PagedRequest
    {
        public string? Name { get; set; }
        public bool? IsOpen { get; set; }
        public int? OwnerId { get; set; }
        public int? MinCapacity { get; set; }

    }
}
