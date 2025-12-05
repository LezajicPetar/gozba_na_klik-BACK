using gozba_na_klik.Dtos.Pagination;
using gozba_na_klik.Enums;

namespace gozba_na_klik.Dtos.Queries
{
    public class UserQuery : PagedRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Role? Role { get; set; }

    }
}
