using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class RestaurantRepository : IRestaurantRepository
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
                .Include(r => r.Menu)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Restaurant> AddAsync(Restaurant restaurant)
        {
            await _dbContext.Restaurants.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant?> UpdateAsync(Restaurant restaurant)
        {
            _dbContext.Restaurants.Update(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant?> DeleteAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id);

            if (restaurant == null) return null;


            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();

            return restaurant;
        }

        public async Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(int ownerId)
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .Include(r => r.Menu)
                .Where(r => r.OwnerId == ownerId)
                .OrderBy(r => r.Name)
                .ToListAsync();
        }
        public async Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId)
        {
            var menuItem = await _dbContext.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.RestaurantId == restaurantId);

            if (menuItem == null) return false;

            _dbContext.MenuItems.Remove(menuItem);

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<MenuItem?> UpdateMenuItemAsync(int restaurantId, MenuItem item)
        {
            var existing = await _dbContext.MenuItems
                   .FirstOrDefaultAsync(m => m.Id == item.Id && m.RestaurantId == restaurantId);

            if (existing == null) return null;

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Price = item.Price;
            existing.PhotoPath = string.IsNullOrWhiteSpace(item.PhotoPath)
                ? existing.PhotoPath
                : item.PhotoPath;

            await _dbContext.SaveChangesAsync();

            return existing;
        }
        public async Task<Restaurant> CreateAsync(Restaurant rest)
        {
            throw new NotImplementedException();
        }
    }
}

