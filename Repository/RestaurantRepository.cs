using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly GozbaDbContext _db;
        public RestaurantRepository(GozbaDbContext db) => _db = db;

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            return await _db.Restaurants.Include(r => r.Menu).ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllWithOwnersAsync()
            => await _db.Restaurants.Include(r => r.Owner).ToListAsync();

        public async Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(int ownerId)
        {
            return await _db.Restaurants
                .Include(r => r.Owner)
                .Where(r => r.OwnerId == ownerId)
                .AsNoTracking()
                .Include(r => r.Menu)
                .ToListAsync();
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
            => await _db.Restaurants.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Restaurant?> GetByIdWithOwnerAsync(int id)
            => await _db.Restaurants.Include(r => r.Owner).FirstOrDefaultAsync(r => r.Id == id);

        public async Task<Restaurant> CreateAsync(Restaurant entity)
        {
            _db.Restaurants.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant> UpdateAsync(Restaurant entity)
        {
            _db.Restaurants.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Restaurant?> DeleteAsync(int id)
        {
            var e = await _db.Restaurants.FindAsync(id);
            if (e == null) return null;
            _db.Restaurants.Remove(e);
            await _db.SaveChangesAsync();
            return e;
        }

        //Work times
        public async Task<List<RestaurantWorkTime>> GetWorkTimesAsync(int restaurantId)
            => await _db.RestaurantWorkTimes
                .Where(w => w.RestaurantId == restaurantId)
                .OrderBy(w => w.DayOfWeek)
                .ToListAsync();

        public async Task SetWorkTimesAsync(int restaurantId, IEnumerable<RestaurantWorkTime> times)
        {
            var existing = _db.RestaurantWorkTimes.Where(w => w.RestaurantId == restaurantId);
            _db.RestaurantWorkTimes.RemoveRange(existing);
            foreach (var t in times)
            {
                t.RestaurantId = restaurantId;
                _db.RestaurantWorkTimes.Add(t);
            }
            await _db.SaveChangesAsync();
        }

        //Exception dates
        public async Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int restaurantId)
            => await _db.RestaurantExceptionDates
                .Where(e => e.RestaurantId == restaurantId)
                .OrderBy(e => e.Date)
                .ToListAsync();

        public async Task<RestaurantExceptionDate> AddExceptionAsync(RestaurantExceptionDate ex)
        {
            await _db.RestaurantExceptionDates.AddAsync(ex);
            await _db.SaveChangesAsync();
            return ex;
        }

        public async Task<bool> DeleteExceptionAsync(int exceptionId)
        {
            var e = await _db.RestaurantExceptionDates.FindAsync(exceptionId);
            if (e == null) return false;
            _db.RestaurantExceptionDates.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId)
        {
            var menuItem = await _db.MenuItems
                .FirstOrDefaultAsync(m => m.Id == menuItemId && m.RestaurantId == restaurantId);

            if (menuItem == null) return false;

            _db.MenuItems.Remove(menuItem);

            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<MenuItem?> UpdateMenuItemAsync(int restaurantId, MenuItem item)
        {
            var existing = await _db.MenuItems
                   .FirstOrDefaultAsync(m => m.Id == item.Id && m.RestaurantId == restaurantId);

            if (existing == null) return null;

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Price = item.Price;
            existing.PhotoPath = string.IsNullOrWhiteSpace(item.PhotoPath)
                ? existing.PhotoPath
                : item.PhotoPath;

            await _db.SaveChangesAsync();

            return existing;
        }
    }
}

