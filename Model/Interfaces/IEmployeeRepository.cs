using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Model.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<User?> GetByIdWithRestaurantAsync(int id, CancellationToken ct = default);
    }
}
