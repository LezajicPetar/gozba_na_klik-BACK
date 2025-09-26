using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Service
{
    public class AuthService
    {
        private readonly GozbaDbContext _dbContext;

        public AuthService(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            return user is null ? null : user;
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
