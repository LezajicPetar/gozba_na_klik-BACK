using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class AllergenRepository
    {
        private readonly GozbaDbContext _dbContext;

        public AllergenRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Allergen?> GetByNameAsync(string name)
        {
            return await _dbContext.Allergens.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<ICollection<Allergen>?> GetAllAsync()
        {
            return await _dbContext.Allergens.ToListAsync();
        }
    }
}
