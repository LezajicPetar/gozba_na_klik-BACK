using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Repository;
using gozba_na_klik.Service.External;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace gozba_na_klik.Service.Implementations
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;
        private readonly TokenService _tokenService;

        public AuthService(UserRepository userRepo, TokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user is null) return null;

            // verifikacija lozinke
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var token = _tokenService.Generate(user);
            var userDto = UserDto.createDto(user);

            return new AuthResponseDto { Token = token, User = userDto };
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // BUG FIX: pre je uvek vraćao null
            return await _userRepo.GetByEmailAsync(email);
        }
        public async Task<User?> RegisterUserAsync(User u)
        {
            var user = await _userRepo.AddUserAsync(u);

            return user;
        }



        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
