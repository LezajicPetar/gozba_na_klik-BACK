namespace gozba_na_klik.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
