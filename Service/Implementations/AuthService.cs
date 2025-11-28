using AutoMapper;
using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Repository;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace gozba_na_klik.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserRepository userRepo, IMapper mapper, ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var email = (dto.Email ?? "").Trim().ToLowerInvariant();
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                return null;

            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid)
                return null;

            _logger.LogInformation("User {UserId} logged in.", user.Id);

            return _mapper.Map<UserDto>(user);
        }
        public Task<User?> GetByEmailAsync(string email)
            => _userRepo.GetByEmailAsync(email);
        public async Task<User?> RegisterUserAsync(User u, string rawPassword)
        {
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            var user = await _userRepo.CreateAsync(u);

            _logger.LogInformation("User {UserId} registered.", user.Id);

            return user;
        }
        public Task LogoutAsync() => Task.CompletedTask;
    }
}
