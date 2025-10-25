using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantInputDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = default!;
        [Required]
        public int OwnerId { get; set; }
        public string? Photo { get; set; }

    }
}
