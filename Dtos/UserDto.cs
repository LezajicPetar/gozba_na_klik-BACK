namespace gozba_na_klik.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? ProfilePicture { get; set; }

        public List<string> Allergens { get; set; } = new List<string>();
    }
}
