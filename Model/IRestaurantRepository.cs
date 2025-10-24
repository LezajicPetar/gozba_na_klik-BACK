using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace gozba_na_klik.Model
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<Restaurant> AddAsync(Restaurant restaurant);
        Task<Restaurant?> UpdateAsync(Restaurant restaurant);
        Task<bool> DeleteAsync(int id);

        Task<List<Restaurant>> GetByOwnerAsync(int ownerId);

        // Work times
        Task<List<RestaurantWorkTime>> GetWorkTimesAsync(int restaurantId);
        Task SetWorkTimesAsync(int restaurantId, IEnumerable<RestaurantWorkTime> times);

        // Exception dates (radni izuzeci)
        Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int restaurantId);
        Task<RestaurantExceptionDate> AddExceptionAsync(RestaurantExceptionDate ex);
        Task<bool> DeleteExceptionAsync(int exceptionId);
    }
}
