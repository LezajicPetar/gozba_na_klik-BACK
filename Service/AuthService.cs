using AutoMapper;
using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace gozba_na_klik.Service
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;
        private readonly IMapper _mapper;

        public AuthService(UserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var email = (dto.Email ?? "").Trim().ToLowerInvariant();
            //pronalazenje user-a po email-u
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
                return null;
            //proveravanje sifre
            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid)
                return null;
            //mapiranje u DTO i vracanje
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepo.GetByEmailAsync(email);
        }
        public async Task<User?> RegisterUserAsync(User u)
        {
            var user = await _userRepo.CreateAsync(u);
            return user;
        }



        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
