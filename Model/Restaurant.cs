namespace gozba_na_klik.Model
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }
        public string? Photo { get; set; }
        public ICollection<MenuItem>? Menu { get; set; }

        public User Owner { get; set; }
        public int OwnerId { get; set; }
    }
}
