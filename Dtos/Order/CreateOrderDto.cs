namespace gozba_na_klik.Dtos.Order
{
    public class CreateOrderDto
    {
        public int RestaurantId { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
