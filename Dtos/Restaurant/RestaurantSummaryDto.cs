using gozba_na_klik.Dtos.MenuItems;
using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Photo { get; set; }
        public bool isPublished { get; set; } = false;
        public List<MenuItemDto>? Menu { get; set; }
    }
}
