using System.ComponentModel.DataAnnotations;
using gozba_na_klik.Enums;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace gozba_na_klik.DtosAdmin
{
    public class CreateUserDto
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }
        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }
    }
}
