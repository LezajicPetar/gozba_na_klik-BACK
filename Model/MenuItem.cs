using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace gozba_na_klik.Model
{
    [Table("MenuItems")]
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PhotoPath { get; set; } = string.Empty;

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

    }
}
