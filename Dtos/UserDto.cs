using gozba_na_klik.Model;

namespace gozba_na_klik.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? ProfilePicture { get; set; }

        public List<string>? Allergens { get; set; } = new List<string>();


        public static UserDto createDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                ProfilePicture = user.ProfilePicture,
                Allergens = user.UserAllergens.Select(ua => ua.Allergen.Name).ToList()
                
            };
        }
    }
}
