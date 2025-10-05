using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class UserRepository
    {
        private readonly GozbaDbContext _dbContext;

        public UserRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                    .Include(u => u.UserAllergens)
                    .ThenInclude(ua => ua.Allergen)
                    .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.Users
            .Include(u => u.UserAllergens)
            .ThenInclude(ua => ua.Allergen)
            .FirstOrDefaultAsync(u => u.Id == user.Id);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users
                    .Include(u => u.UserAllergens)
                    .ThenInclude(ua => ua.Allergen) 
                    .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
