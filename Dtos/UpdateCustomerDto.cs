namespace gozba_na_klik.Dtos
{
    public class UpdateCustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> Allergens { get; set; } = new List<string>();
    }
}
