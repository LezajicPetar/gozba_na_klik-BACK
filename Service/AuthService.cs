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
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            //VALIDACIJA ZA PASSWORD OVDE IDE
            var userDto = _mapper.Map<UserDto>(user);

            return user is null ? null : userDto;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            return null;
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
