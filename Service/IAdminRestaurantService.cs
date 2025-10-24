using gozba_na_klik.Dtos.Restaurants;

namespace gozba_na_klik.Service
{
    public interface IAdminRestaurantService
    {
        Task<IEnumerable<RestaurantDto>> GetAllAsync();
        Task<RestaurantDto> GetByIdAsync(int id);
        Task<RestaurantDto> CreateAsync(RestaurantInputDto inputDto);
        Task<RestaurantDto> UpdateAsync(int id,RestaurantInputDto inputDto);
        Task DeleteAsync(int id);
    }
}
