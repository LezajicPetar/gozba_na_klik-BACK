namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantUpsertDto
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }
    }
}

