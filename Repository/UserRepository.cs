using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Enums;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly GozbaDbContext _dbContext;

        public UserRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users
                .Include(u => u.UserAllergens)
                    .ThenInclude(ua => ua.Allergen)
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> CreateAsync(User entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Username))
            {
                if (!string.IsNullOrWhiteSpace(entity.Email) && entity.Email.Contains("@"))
                    entity.Username = entity.Email.Split('@')[0];
                else
                    entity.Username = (entity.FirstName + entity.LastName).ToLower();

                if (string.IsNullOrWhiteSpace(entity.Username))
                    entity.Username = "user" + Guid.NewGuid().ToString("N").Substring(0, 6);
            }

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _dbContext.Users.Update(entity);
            await _dbContext.SaveChangesAsync();

            return (await _dbContext.Users
                .Include(u => u.UserAllergens)
                    .ThenInclude(ua => ua.Allergen)
                .FirstOrDefaultAsync(u => u.Id == entity.Id))!;
        }

        public async Task<User?> DeleteAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        #endregion

        #region CUSTOM Metode
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
                    .Include(u => u.Addresses)
                    .FirstOrDefaultAsync(u => u.Email == email);
        }

        //za dobavljanje vlasnika restorana
        public async Task<IEnumerable<User>> GetOwnersAsync()
        {
            return await _dbContext.Users
                .Where(u => u.Role == Role.RestaurantOwner)
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}