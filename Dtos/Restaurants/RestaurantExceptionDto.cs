namespace gozba_na_klik.Dtos.Restaurants
{
    public class RestaurantExceptionDto
    {
        public int Id { get; set; }
        public string Date { get; set; } = default!;
        public string? Reason { get; set; }
    }
}
