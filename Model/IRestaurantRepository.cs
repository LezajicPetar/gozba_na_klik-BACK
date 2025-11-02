using gozba_na_klik.Dtos.Restaurants;

namespace gozba_na_klik.Model
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
         Task<IEnumerable<Restaurant>> GetAllByOwnerAsync(int ownerId);

         Task<bool> DeleteMenuItemAsync(int restaurantId, int menuItemId);

         Task<MenuItem> UpdateMenuItemAsync(int restaurantId, MenuItem item);


    }
}
