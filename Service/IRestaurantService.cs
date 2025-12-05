using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Dtos.Pagination;
using gozba_na_klik.Dtos.Queries;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;

namespace gozba_na_klik.Service
{
    public interface IRestaurantService
    {
         Task DeleteMenuItemAsync(int restaurantId, int menuItemId);
         Task<IEnumerable<RestaurantDto>> GetAllByOwnerAsync(int ownerId);
         Task<UpdateMenuItemDto> UpdateMenuItemAsync(int restorauntId, UpdateMenuItemDto item);
         Task<IEnumerable<RestaurantDto>> GetAllAsync();

        Task<PagedResult<RestaurantDto>> GetPagedRestaurantsAsync(RestaurantQuery query);

        Task<IEnumerable<RestaurantDto>> GetMostRecentByUserAsync(int userId);
        Task<IEnumerable<RestaurantDto>> GetFavouriteByUserAsync(int userId);
        Task<IEnumerable<RestaurantDto>> GetTopRatedAsync();

    }
}
