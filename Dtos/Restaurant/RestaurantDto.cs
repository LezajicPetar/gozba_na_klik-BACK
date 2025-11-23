using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Model;

namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Photo { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public IEnumerable<MenuItemDto>? Menu { get; set; }

    }
}
