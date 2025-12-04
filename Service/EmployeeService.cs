using AutoMapper;
using gozba_na_klik.Dtos;
using gozba_na_klik.Model;
using BCrypt.Net;
using gozba_na_klik.Dtos.Employee;

namespace gozba_na_klik.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            var list = await _repo.GetByRestaurantIdAsync(restaurantId);
            return _mapper.Map<List<EmployeeDto>>(list);
        }

        public async Task<EmployeeDto?> GetAsync(int id)
        {
            var e = await _repo.GetAsync(id);
            return _mapper.Map<EmployeeDto?>(e);
        }

        public async Task<EmployeeDto> CreateAsync(int restaurantId, EmployeeCreateDto dto)
        {

            var existing = await _repo.GetByRestaurantIdAsync(restaurantId);
            if (existing.Any(e => e.Email == dto.Email.Trim().ToLowerInvariant()))
            {
                throw new Exception("Email vec postoji.");
            }

            var employee = new Employee
            {
                RestaurantId = restaurantId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email.Trim().ToLowerInvariant(),
                Position = dto.Position,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Status = EmployeeStatus.Active
            };

            var created = await _repo.CreateAsync(employee);
            return _mapper.Map<EmployeeDto>(created);
        }

        public async Task<EmployeeDto?> UpdateAsync(int employeeId, EmployeeUpdateDto dto)
        {
            var employee = await _repo.GetAsync(employeeId);

            if (employee == null)
                return null;

            var normalizedEmail = dto.Email.Trim().ToLower();
            var existing = await _repo.GetByEmailAsync(normalizedEmail);

            if (existing != null && existing.Id != employee.Id)
                throw new Exception("Email već postoji.");

            _mapper.Map(dto, employee);

            await _repo.UpdateAsync(employee);

            return _mapper.Map<EmployeeDto>(employee);
        }

        public async Task<EmployeeDto?> ToggleStatusAsync(int employeeId)
        {
            var employee = await _repo.GetAsync(employeeId);

            if (employee == null)
                return null;

            await _repo.ToggleStatusAsync(employeeId);

            employee = await _repo.GetAsync(employeeId);

            return _mapper.Map<EmployeeDto>(employee);
        }

    }
}
