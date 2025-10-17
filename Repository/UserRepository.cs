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
        #region USER
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
            if (string.IsNullOrWhiteSpace(user.Username))
            {
                if (!string.IsNullOrWhiteSpace(user.Email) && user.Email.Contains("@"))
                {
                    user.Username = user.Email.Split('@')[0];
                }
                else
                {
                    user.Username = (user.FirstName + user.LastName).ToLower();
                }

                if (string.IsNullOrWhiteSpace(user.Username))
                {
                    user.Username = "user" + Guid.NewGuid().ToString("N").Substring(0, 6);
                }
            }

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

        #endregion

        #region OWNER

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

        #endregion
    }
}
