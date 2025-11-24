namespace gozba_na_klik.Dtos.MenuItem
{
    public class CreateMenuItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string PhotoPath { get; set; } = string.Empty;
    }
}
