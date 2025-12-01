using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;


namespace gozba_na_klik.Service
{
    public class UserTokenService
    {
        private readonly UserTokenRepository _repo;

        public UserTokenService(UserTokenRepository repo)
        {
            _repo = repo;
        }

        public string GenerateRawToken()
        {
            return Guid.NewGuid().ToString("N");
        }

        public string HashToken(string raw)
        {
            return raw;
        }

        public async Task<UserToken> CreateActivationToken(int userId)
        {
            var raw = GenerateRawToken();
            var hash = HashToken(raw);

            var token = new UserToken
            {
                UserId = userId,
                Type = UserTokenType.Activation,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await _repo.CreateAsync(token);

            return new UserToken
            {
                Id = token.Id,
                UserId = token.UserId,
                TokenHash = raw,
                Type = token.Type,
                ExpiresAt = token.ExpiresAt,
                CreatedAt = token.CreatedAt
            };
        }

        public async Task<UserToken> CreateResetPasswordToken(int userId)
        {
            var raw = GenerateRawToken();
            var hash = HashToken(raw);

            var token = new UserToken
            {
                UserId = userId,
                Type = UserTokenType.ResetPassword,
                TokenHash = hash,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            await _repo.CreateAsync(token);

            return new UserToken
            {
                Id = token.Id,
                UserId = token.UserId,
                TokenHash = raw,
                Type = token.Type,
                ExpiresAt = token.ExpiresAt,
                CreatedAt = token.CreatedAt
            };
        }

        public async Task<UserToken?> ValidateToken(string raw, UserTokenType type)
        {
            var hash = HashToken(raw);
            return await _repo.GetValidTokenAsync(hash, type);
        }
    }
}
