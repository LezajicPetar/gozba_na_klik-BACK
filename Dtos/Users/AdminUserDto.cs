using gozba_na_klik.Enums;

namespace gozba_na_klik.DtosAdmin
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
