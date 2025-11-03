using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;

namespace gozba_na_klik.Service.Interfaces
{
    public interface IAdminUserService
    {
        Task<IEnumerable<AdminUserDto>> GetAllAsync();
        Task<IEnumerable<OwnerDto>> GetOwnersAsync();
        Task<AdminUserDto> CreateAsync(CreateUserDto user);
    }
}
