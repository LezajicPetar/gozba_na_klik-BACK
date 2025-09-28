﻿using gozba_na_klik.Data;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace gozba_na_klik.Service
{
    public class AuthService
    {
        private readonly UserRepository _userRepo;

        public AuthService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);

            //VALIDACIJA ZA PASSWORD OVDE IDE

            return user;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email);

            return null;
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
