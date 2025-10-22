using gozba_na_klik.Enums;
using System.Net;

namespace gozba_na_klik.Model.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } = default!;
        public Role Role { get; set; } = Role.Customer;
        public bool IsSuspended { get; set; } = false;

        public string? ProfilePicture { get; set; }

        public ICollection<Address>? Addresses { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; } = new List<UserAllergen>();
        public ICollection<WorkTime>? WorkTimes { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
    }
}
