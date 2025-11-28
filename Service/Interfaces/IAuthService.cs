using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Service.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto?> LoginAsync(LoginDto dto);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> RegisterUserAsync(User user, string rawPassword);
        Task LogoutAsync();
    }
}

