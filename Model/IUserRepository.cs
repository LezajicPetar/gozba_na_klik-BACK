using gozba_na_klik.Dtos.Users;

namespace gozba_na_klik.Model
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByNameAsync(string firstName, string lastName);
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetOwnersAsync();
    }
}
