using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Enums;
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
        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByNameAsync(string firstName, string lastName)
        {
            return await _dbContext.Users.AnyAsync(u => u.FirstName == firstName && u.LastName == lastName);
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

        //za dobavljanje vlasnika restorana
        public async Task<List<OwnerDto>> GetOwnersAsync()
        {
            return await _dbContext.Users
                .Where(u => u.Role == Role.RestaurantOwner)
                .Select(u => new OwnerDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();
        }
    }
}
