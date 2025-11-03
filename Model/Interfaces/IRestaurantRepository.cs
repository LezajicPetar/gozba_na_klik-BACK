using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using gozba_na_klik.Model.Entities;

namespace gozba_na_klik.Model.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        // ADMIN funkcionalnosti
        Task<Restaurant?> GetByIdWithOwnerAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllWithOwnersAsync();

        // OWNER funkcionalnosti
        Task<IEnumerable<Restaurant>> GetByOwnerAsync(int ownerId);

        // Radno vreme
        Task<List<RestaurantWorkTime>> GetWorkTimesAsync(int restaurantId);
        Task SetWorkTimesAsync(int restaurantId, IEnumerable<RestaurantWorkTime> times);

        // Izuzeci (neradni dani)
        Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int restaurantId);
        Task<RestaurantExceptionDate> AddExceptionAsync(RestaurantExceptionDate ex);
        Task<bool> DeleteExceptionAsync(int exceptionId);
    }
}
