namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantWorkTimeDto
    {
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public string? Open { get; set; }
        public string? Close { get; set; }
        public bool IsClosed { get; set; }
    }
}
