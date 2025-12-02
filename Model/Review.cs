using System.ComponentModel.DataAnnotations;

namespace gozba_na_klik.Model
{
    public class Review
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
