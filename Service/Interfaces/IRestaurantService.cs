using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Dtos.Review;
using gozba_na_klik.Model;

namespace gozba_na_klik.Service.Interfaces
{
    public interface IRestaurantService
    {
        Task DeleteMenuItemAsync(int restaurantId, int menuItemId);
        Task<IEnumerable<RestaurantDto>> GetAllByOwnerAsync(int ownerId);
        Task<UpdateMenuItemDto> UpdateMenuItemAsync(int restorauntId, UpdateMenuItemDto item);
        Task<IEnumerable<RestaurantDto>> GetAllAsync();


        Task<IEnumerable<RestaurantDto>> GetMostRecentByUserAsync(int userId);
        Task<IEnumerable<RestaurantDto>> GetFavouriteByUserAsync(int userId);
        Task<IEnumerable<RestaurantDto>> GetTopRatedAsync();
    }
}
