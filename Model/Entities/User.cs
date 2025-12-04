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


        // Status kurira AZ
        public bool isActive { get; set; } = true;
        public bool IsBusy { get; set; } = false;
        public int? CurrentOrderId { get; set; }

        public string? ProfilePicture { get; set; }

        public bool IsActive { get; set; } = false;

        public ICollection<Address>? Addresses { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; } = new List<UserAllergen>();
        public ICollection<WorkTime>? WorkTimes { get; set; }
        public ICollection<Restaurant>? Restaurants { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public int? EmployeeRestaurantId { get; set; }
        public Restaurant? EmployeeRestaurant { get; set; }
    }
}
