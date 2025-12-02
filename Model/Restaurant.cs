namespace gozba_na_klik.Model
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }
        public string? Photo { get; set; }
        public ICollection<MenuItem>? Menu { get; set; }

        public User? Owner { get; set; }
        public int OwnerId { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<RestaurantWorkTime>? WorkTimes { get; set; } = new List<RestaurantWorkTime>();
        public ICollection<RestaurantExceptionDate>? ExceptionDates { get; set; } = new List<RestaurantExceptionDate>();
    }
}
