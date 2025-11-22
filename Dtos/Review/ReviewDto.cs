namespace gozba_na_klik.Dtos.Review
{
    public class ReviewDto
    {
        public string Username { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
