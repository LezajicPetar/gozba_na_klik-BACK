namespace gozba_na_klik.Model
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetByRestaurantIdAsync(int restaurantId);

        Task<Employee?> GetAsync(int id);

        Task<Employee?> GetByEmailAsync(string email);


        Task<Employee> CreateAsync(Employee employee);

        Task UpdateAsync(Employee employee);

        Task ToggleStatusAsync(int id);
    }
}
