namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantOwner
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        
        public string? Phone { get; set; }

        public int? Capacity { get; set; }
    }

    public class RestaurantSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? PhotoUrl { get; set; }
    }
}
