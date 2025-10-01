namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantInputDto
    {
        public string Name { get; set; } = default!;
        public int OwnerId { get; set; }
        public string? Photo { get; set; }

    }
}
