using gozba_na_klik.Model.Entities;
using System.Threading.Tasks;
namespace gozba_na_klik.Model.Interfaces
{
    public interface ICourierRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int userId);
        Task<bool> ExistsByUserIdAsync(int userId);
        Task<User> EnsureCourierRoleAsync(int userId);
    }
}
