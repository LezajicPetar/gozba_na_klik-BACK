using AutoMapper;
using gozba_na_klik.Dtos;
using BCrypt.Net;
using gozba_na_klik.Dtos.Employee;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Enums;

namespace gozba_na_klik.Service
{
    public class EmployeeMenagementService : IEmployeeMenagementService
    {
        private readonly IEmployeeMenagementRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _users;

        public EmployeeMenagementService(IEmployeeMenagementRepository repo, IMapper mapper, IUserRepository users)
        {
            _repo = repo;
            _mapper = mapper;
            _users = users;
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
            var normalizedEmail = dto.Email.Trim().ToLowerInvariant();

            // Globalna provera email-a
            if (await _users.ExistsByEmailAsync(normalizedEmail))
                throw new Exception("Email već postoji u sistemu!");

            // Lokalna provera u tom restoranu
            var existing = await _repo.GetByRestaurantIdAsync(restaurantId);
            if (existing.Any(e => e.Email == normalizedEmail))
                throw new Exception("Email već postoji u ovom restoranu.");

            // Kreiranje USER-a
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = normalizedEmail,
                Username = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = Role.Employee,
                EmployeeRestaurantId = restaurantId,
                IsActive = true,
                IsBusy = false
            };

            await _users.CreateAsync(user);


            // Kreiranje EmployeeMenagement entiteta
            var employee = new EmployeeMenagement
            {
                RestaurantId = restaurantId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = normalizedEmail,
                Position = dto.Position,
                PasswordHash = user.PasswordHash,
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
