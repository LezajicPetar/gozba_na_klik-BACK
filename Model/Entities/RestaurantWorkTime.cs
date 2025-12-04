namespace gozba_na_klik.Model.Entities
{
    public class RestaurantWorkTime
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;

        //0=Sunday...6=Saturaday

        public int DayOfWeek { get; set; }
        public TimeSpan Open { get; set; }
        public TimeSpan Close { get; set; }
        public bool IsClosed { get; set; }
    }
}
