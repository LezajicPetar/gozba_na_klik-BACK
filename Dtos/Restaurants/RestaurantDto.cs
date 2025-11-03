namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }
        public string? Photo { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
    }
}
