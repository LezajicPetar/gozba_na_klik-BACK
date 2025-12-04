using System.ComponentModel.DataAnnotations.Schema;

namespace gozba_na_klik.Model.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        public decimal Price { get; set; }
    }
}
