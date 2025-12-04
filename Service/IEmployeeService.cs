using gozba_na_klik.Dtos.Employee;

namespace gozba_na_klik.Service
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetByRestaurantIdAsync(int restaurantId);
        Task<EmployeeDto?> GetAsync(int id);
        Task<EmployeeDto> CreateAsync(int restaurantId, EmployeeCreateDto dto);
        Task<EmployeeDto> UpdateAsync(int id, EmployeeUpdateDto dto);
        Task<EmployeeDto?> ToggleStatusAsync(int employeeId);

    }
}
