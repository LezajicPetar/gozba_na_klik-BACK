using gozba_na_klik.Data;
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

        public async Task<List<Restaurant>> GetAllAsync()
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

        public async Task<bool> DeleteAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id);
            if (restaurant == null)
                return false;

            _dbContext.Restaurants.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Restaurant>> GetByOwnerAsync(int ownerId)
        {
            return await _dbContext.Restaurants
                .Include(r => r.Owner)
                .Where(r => r.OwnerId == ownerId)
                .OrderBy(r =>  r.Name)
                .ToListAsync();
        }

        public async Task<List<RestaurantWorkTime>> GetWorkTimesAsync(int restaurantId)
        {
            return await _dbContext.RestaurantWorkTimes
                .Where(w => w.RestaurantId == restaurantId)
                .OrderBy(w => w.DayOfWeek)
                .ToListAsync();
        }

        public async Task SetWorkTimesAsync(int restaurantId, IEnumerable<RestaurantWorkTime> times)
        {
            var existing = _dbContext.RestaurantWorkTimes.Where(w => w.RestaurantId == restaurantId);
            _dbContext.RestaurantWorkTimes.RemoveRange(existing);
            foreach (var t in times) { t.RestaurantId = restaurantId; _dbContext.RestaurantWorkTimes.Add(t); }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int restaurantId)
        {
            return await _dbContext.RestaurantExceptionDates
                .Where(e => e.RestaurantId == restaurantId)
                .OrderBy(e => e.Date)
                .ToListAsync();
        }

        public async Task<RestaurantExceptionDate> AddExceptionAsync(RestaurantExceptionDate ex)
        {
            try
            {
                await _dbContext.RestaurantExceptionDates.AddAsync(ex);
                await _dbContext.SaveChangesAsync();
                return ex;
            }
            catch (Exception e)
            {
                Console.WriteLine("❌ DB ERROR: " + e.Message);
                if (e.InnerException != null)
                    Console.WriteLine("💥 INNER: " + e.InnerException.Message);
                throw;
            }
        }


        public async Task<bool> DeleteExceptionAsync(int exceptionId)
        {
            var e = await _dbContext.RestaurantExceptionDates.FindAsync(exceptionId);
            if (e == null) return false;
            _dbContext.RestaurantExceptionDates.Remove(e);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}

