using gozba_na_klik.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gozba_na_klik.Model.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = null!;

        public int CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        public int? CourierId { get; set; }
        public User? Courier { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AddressId { get; set; }
        public Address Address { get; set; } = null!;

        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; } = 200;
        public decimal Total { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.NA_CEKANJU;

        public List<OrderItem> Items { get; set; } = new();
    }
}
