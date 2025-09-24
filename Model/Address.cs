namespace gozba_na_klik.Model
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        public string City { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
    }
}
