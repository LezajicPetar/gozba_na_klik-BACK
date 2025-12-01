using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model;

namespace gozba_na_klik.Service
{
    public interface IAuthService
    {
        Task<UserDto?> LoginAsync(LoginDto dto);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> RegisterUserAsync(User user, string rawPassword);
        Task LogoutAsync();

        Task ActivateAsync(string tokenRaw);

        Task RequestPasswordResetAsync(string email);

        Task ResetPasswordAsync(string rawToken, string newPassword);

        Task<string?> RequestPasswordResetAndReturnToken(string email);

    }
}

