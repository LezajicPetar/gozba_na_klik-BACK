namespace gozba_na_klik.Model
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        // ADMIN funkcionalnosti
        Task<Restaurant?> GetByIdWithOwnerAsync(int id);
        Task<IEnumerable<Restaurant>> GetAllWithOwnersAsync();

        // OWNER funkcionalnosti
        Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(int ownerId);
        Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId);

        Task<MenuItem> UpdateMenuItemAsync(int restaurantId, MenuItem item);

        // Radno vreme
        Task<List<RestaurantWorkTime>> GetWorkTimesAsync(int restaurantId);
        Task SetWorkTimesAsync(int restaurantId, IEnumerable<RestaurantWorkTime> times);

        // Izuzeci (neradni dani)
        Task<List<RestaurantExceptionDate>> GetExceptionsAsync(int restaurantId);
        Task<RestaurantExceptionDate> AddExceptionAsync(RestaurantExceptionDate ex);
        Task<bool> DeleteExceptionAsync(int exceptionId);


        Task<IEnumerable<Restaurant>> GetMostRecentByUserAsync(int userId);
        Task<IEnumerable<Restaurant>> GetFavouritesByUserAsync(int userId);
        Task<IEnumerable<Restaurant>> GetTopRatedAsync();
    }
}
