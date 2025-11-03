using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Model.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByNameAsync(string firstName, string lastName);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetOwnersAsync();
    }
}
