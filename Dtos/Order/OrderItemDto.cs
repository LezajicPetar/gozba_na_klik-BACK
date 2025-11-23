namespace gozba_na_klik.Dtos.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public decimal Price { get; set; }
    }
}
