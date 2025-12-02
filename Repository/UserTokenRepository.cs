using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;
namespace gozba_na_klik.Repository
{
    public class UserTokenRepository
    {
        private readonly GozbaDbContext _context;
        public UserTokenRepository(GozbaDbContext context)
        {
            _context = context;
        }
        public async Task<UserToken> CreateAsync(UserToken token)
        {
            _context.UserTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<UserToken?> GetValidTokenAsync(string tokenHash, UserTokenType type)
        {
            return await _context.UserTokens
                .Include(t => t.User)
                .Where(t => t.TokenHash == tokenHash &&
                            t.Type == type &&
                            t.UsedAt == null &&
                            t.ExpiresAt > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
