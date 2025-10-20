using System.Text.Json.Serialization;

namespace gozba_na_klik.Model
{
    public class RestaurantExceptionDate
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        [JsonIgnore]
        public Restaurant? Restaurant { get; set; }
        public DateTime Date { get; set; }
        public string? Reason { get; set; }

    }
}
