using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Dtos.Review
{
    public class CreateReviewDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
