using gozba_na_klik.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace gozba_na_klik.DtosAdmin
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
