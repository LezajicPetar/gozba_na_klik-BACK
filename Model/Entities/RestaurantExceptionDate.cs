using System.Text.Json.Serialization;

namespace gozba_na_klik.Model.Entities
{
    public class RestaurantExceptionDate
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public DateTime Date { get; set; }
        public string? Reason { get; set; }

    }
}
