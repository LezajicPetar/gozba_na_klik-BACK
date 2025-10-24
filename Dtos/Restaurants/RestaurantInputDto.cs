using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantInputDto
    {
        [Required, StringLength(120)]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
