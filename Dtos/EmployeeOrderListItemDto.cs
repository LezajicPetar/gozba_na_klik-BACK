namespace gozba_na_klik.Dtos
{
    public class EmployeeOrderListItemDto
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int AddressId { get; set; }
        public string AddressText { get; set; } = string.Empty;

        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
