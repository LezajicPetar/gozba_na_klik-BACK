using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore; 

namespace gozba_na_klik.Repository
{
    public class RestaurantRepository : IRepository<Restaurant>
    {
        private readonly GozbaDbContext _dbContext;

        public RestaurantRepository(GozbaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant> UpdateAsync(Restaurant entity)
        {
            _dbContext.Restaurants.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant> CreateAsync(Restaurant entity)
        {
            await _dbContext.Restaurants.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant?> DeleteAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id);
            if (restaurant == null)
                return null;

            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<List<Restaurant>> GetByOwnerAsync(int ownerId)
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .Where(r => r.OwnerId == ownerId)
                .OrderBy(r =>  r.Name)
                .ToListAsync();
        }
    }
}

