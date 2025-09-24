namespace gozba_na_klik.Model
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Photo { get; set; }

        public User Owner { get; set; }
        public int OwnerId { get; set; }
    }
}
