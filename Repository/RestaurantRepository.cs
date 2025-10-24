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
            => await _db.Restaurants.ToListAsync();

        public async Task<IEnumerable<Restaurant>> GetAllWithOwnersAsync()
            => await _db.Restaurants.Include(r => r.Owner).ToListAsync();
        public async Task<IEnumerable<Restaurant>> GetByOwnerAsync(int ownerId)
        {
            return await _db.Restaurants
                .Include(r => r.Owner)
                .Where(r => r.OwnerId == ownerId)
                .AsNoTracking()
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
            await _dbContext.RestaurantExceptionDates.AddAsync(ex);
            await _dbContext.SaveChangesAsync();
            return ex;
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

