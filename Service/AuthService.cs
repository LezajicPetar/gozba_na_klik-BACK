using AutoMapper;
using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using gozba_na_klik.Service.External;
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
        private readonly UserTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthService(UserRepository userRepo, IMapper mapper, ILogger<AuthService> logger, UserTokenService tokenService, IEmailService emailService, IConfiguration config)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _emailService = emailService;
            _config = config;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var email = (dto.Email ?? "").Trim().ToLowerInvariant();
            _logger.LogWarning("LOGIN STEP 1 → email normalized = " + email);
            var user = await _userRepo.GetByEmailAsync(email);

            if (user == null)
            {
                _logger.LogWarning("LOGIN FAIL → user not found in DB");
                return null;
            }

            _logger.LogWarning($"LOGIN STEP 2 → user found: Id={user.Id}, IsActive={user.IsActive}");
            if (!user.IsActive)
            {
                var activationToken = await _tokenService.CreateActivationToken(user.Id);

                var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:5173";
                var activationLink = $"{frontendUrl}/activate?token={activationToken.TokenHash}";
                await _emailService.SendActivationEmailAsync(user.Email, activationLink);

                return null;
            }


            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            _logger.LogWarning($"LOGIN STEP 3 → password match = {isValid}");

            if (!isValid)
            {
                _logger.LogWarning("LOGIN FAIL → WRONG PASSWORD");
                return null;
            }
            _logger.LogWarning("LOGIN OK → return userDto");
            return _mapper.Map<UserDto>(user);
        }
        public Task<User?> GetByEmailAsync(string email)
        {
            var normalized = (email ?? "").Trim().ToLowerInvariant();
            return _userRepo.GetByEmailAsync(normalized);
        }

        public async Task<User?> RegisterUserAsync(User u, string rawPassword)
        {
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            u.IsActive = false;

            var user = await _userRepo.CreateAsync(u);

            _logger.LogInformation("User {UserId} registered.", user.Id);

            var activationToken = await _tokenService.CreateActivationToken(user.Id);

            var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:5173";

            var activationLink = $"{frontendUrl}/activate?token={activationToken.TokenHash}";

            await _emailService.SendActivationEmailAsync(user.Email, activationLink);

            _logger.LogInformation("Activation email sent to {Email}.", user.Email);

            return user;
        }
        public Task LogoutAsync() => Task.CompletedTask;

        public async Task ActivateAsync(string tokenRaw)
        {
            var token = await _tokenService.ValidateToken(tokenRaw, UserTokenType.Activation);

            if (token == null)
            {
                _logger.LogWarning("Invalid or expired activation token used.");
                throw new Exception("Nevalidan ili istekao token za aktivaciju.");
            }

            token.User.IsActive = true;
            token.UsedAt = DateTime.UtcNow;

            await _userRepo.SaveChangesAsync();
            _logger.LogInformation("User {UserId} activated account.", token.UserId);
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var normalizedEmail = (email ?? "").Trim().ToLowerInvariant();
            var user = await _userRepo.GetByEmailAsync(normalizedEmail);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Password reset requested for non-existing or inactive account {Email}.", normalizedEmail);
                return;
            }

            var resetToken = await _tokenService.CreateResetPasswordToken(user.Id);

            var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:5173";
            var resetLink = $"{frontendUrl}/reset-password?token={resetToken.TokenHash}";

            await _emailService.SendResetEmailAsync(user.Email, resetLink);

            _logger.LogInformation("Password reset email sent to {Email}.", user.Email);
        }

        public async Task ResetPasswordAsync(string rawToken, string newPassword)
        {
            var token = await _tokenService.ValidateToken(rawToken, UserTokenType.ResetPassword);

            if (token == null)
            {
                _logger.LogWarning("Invalid or expired reset password token used.");
                throw new Exception("Nevalidan ili istekao token za reset lozinke.");
            }

            var user = token.User;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            token.UsedAt = DateTime.UtcNow;

            await _userRepo.SaveChangesAsync();

            _logger.LogInformation("User {UserId} successfully reset password.", user.Id);
        }

        public async Task<string?> RequestPasswordResetAndReturnToken(string email)
        {
            var normalizedEmail = (email ?? "").Trim().ToLowerInvariant();
            var user = await _userRepo.GetByEmailAsync(normalizedEmail);

            if (user == null || !user.IsActive)
                return null;

            var resetToken = await _tokenService.CreateResetPasswordToken(user.Id);

            var frontendUrl = _config["FrontendUrl"] ?? "http://localhost:5173";
            var resetLink = $"{frontendUrl}/reset-password?token={resetToken.TokenHash}";
            await _emailService.SendResetEmailAsync(user.Email, resetLink);

            return resetToken.TokenHash;
        }



    }
}
