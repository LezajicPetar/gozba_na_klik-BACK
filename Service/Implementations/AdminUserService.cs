using System;
using System.Text.RegularExpressions;
using System.Threading;
using AutoMapper;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Enums;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model.Interfaces;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Service.Interfaces;

namespace gozba_na_klik.Service
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository _users; 
        private readonly IMapper _mapper;
        private readonly ILogger<AdminUserService> _logger;

        public AdminUserService(IUserRepository users, IMapper mapper, ILogger<AdminUserService> logger)
        {
            _users = users;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AdminUserDto>> GetAllAsync()
        {
            _logger.LogDebug("Fetching all users");
            var list = await _users.GetAllAsync();
            return _mapper.Map<IEnumerable<AdminUserDto>>(list);
        }

        public async Task<AdminUserDto> CreateAsync(CreateUserDto dto)
        {
            _logger.LogInformation("Admin creating user Email={Email} Role={Role}", dto.Email, dto.Role);
            
            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
            {
                _logger.LogWarning("Invalid email format: {Email}", dto.Email);
                throw new BadRequestException("Invalid email format.");
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            {
                _logger.LogWarning("Missing name for Email={Email}", dto.Email);
                throw new BadRequestException("First and last name are required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new BadRequestException("Password should be at least 6 characters long.");

            var passwordPattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z\d]).+$";
            if (!Regex.IsMatch(dto.Password, passwordPattern))
                throw new BadRequestException("Password must contain letters, numbers, and at least one special character.");

            if (await _users.ExistsByEmailAsync(dto.Email))
                throw new BadRequestException("The email already exists.");

            if (await _users.ExistsByNameAsync(dto.FirstName, dto.LastName))
                throw new BadRequestException("User with this name already exists.");

            if (dto.Role != Role.Courier && dto.Role != Role.RestaurantOwner)
                throw new BadRequestException("The role must be Courier or RestaurantOwner.");
            
            var entity = _mapper.Map<User>(dto);

            entity.Email = dto.Email.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(entity.Username))
                entity.Username = entity.Email;

            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            entity.IsSuspended = false;
            entity.isActive = true;
            entity.IsBusy = false;
            entity.CurrentOrderId = null;

            var created = await _users.CreateAsync(entity);

            _logger.LogInformation("Created user Id={Id}", created.Id);

            return _mapper.Map<AdminUserDto>(created);
        }

        public async Task<IEnumerable<OwnerDto>> GetOwnersAsync()
        {
            _logger.LogDebug("Fetching restaurant owners");

            var owners = await _users.GetOwnersAsync();
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }
    }
}
