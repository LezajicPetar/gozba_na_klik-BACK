using gozba_na_klik.Data;
using gozba_na_klik.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace gozba_na_klik.Repository
{
    public class UserAllergenRepository
    {
        private readonly GozbaDbContext _dbContext;

        public UserAllergenRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<Allergen>?> GetAllByUserAsync(int userId)
        {
            return await _dbContext.UserAllergens
                    .Where(ua => ua.UserId == userId)
                    .Include(ua => ua.Allergen)
                    .Select(ua => ua.Allergen)
                    .ToListAsync();
        }

        public async Task ReplaceUserAllergens(User user, IEnumerable<UserAllergen> newUserAllergens)
        {
            var existing = _dbContext.UserAllergens.Where(ua => ua.UserId == user.Id);
            _dbContext.UserAllergens.RemoveRange(existing);

            if (newUserAllergens.Any())
            {
                await _dbContext.UserAllergens.AddRangeAsync(newUserAllergens);
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
