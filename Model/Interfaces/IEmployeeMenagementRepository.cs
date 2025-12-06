using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Model.Interfaces
{
    public interface IEmployeeMenagementRepository
    {
        Task<List<EmployeeMenagement>> GetByRestaurantIdAsync(int restaurantId);

        Task<EmployeeMenagement?> GetAsync(int id);

        Task<EmployeeMenagement?> GetByEmailAsync(string email);


        Task<EmployeeMenagement> CreateAsync(EmployeeMenagement employee);

        Task UpdateAsync(EmployeeMenagement employee);

        Task ToggleStatusAsync(int id);
    }
}
