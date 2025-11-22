using AutoMapper;
using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Exceptions;
using gozba_na_klik.Model;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(IRestaurantRepository repo, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _restaurantRepo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await _restaurantRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }
        public async Task<IEnumerable<RestaurantDto>> GetAllByOwnerAsync(int ownerId)
        {
            _logger.LogInformation("Fetching all owners restaurants ...");

            var restaurants = await _restaurantRepo.GetAllByOwnerAsync(ownerId);

            _logger.LogInformation("Fetched {Count} restaurants.", restaurants.Count());

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }
        public async Task<RestaurantDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching restaurant with ID {RestaurantId}", id);

            var restaurant = await EnsureRestaurantExistsAsync(id);

            _logger.LogInformation("Restaurant with ID {RestaurantId} retrieved successfully.", id);

            return _mapper.Map<RestaurantDto>(restaurant);
        }
        public async Task DeleteMenuItemAsync(int restaurantId, int menuItemId)
        {
            _logger.LogInformation("Deleting menu item {MenuItemId} for restaurant {RestaurantId}", menuItemId, restaurantId);

            await EnsureRestaurantExistsAsync(restaurantId);

            var deleted = await _restaurantRepo.DeleteMenuItemAsync(restaurantId, menuItemId);

            if (!deleted) throw new NotFoundException("MenuItem", menuItemId);

            _logger.LogInformation("Menu item {MenuItemId} deleted successfully from restaurant {RestaurantId}.", menuItemId, restaurantId);
        }
        public async Task<UpdateMenuItemDto> UpdateMenuItemAsync(int restaurantId, UpdateMenuItemDto item)
        {
            _logger.LogInformation("Updating menu item: {Name} for restaurant {RestaurantId}", item.Name, restaurantId);

            if (restaurantId != item.RestaurantId)
                throw new BadRequestException("Jelo ne pripada restoranu (ID-jevi se ne podudaraju).");

            var restaurant = await EnsureRestaurantExistsAsync(restaurantId);

            var menuItem = _mapper.Map<MenuItem>(item);

            var updated = await _restaurantRepo.UpdateMenuItemAsync(restaurantId, menuItem);

            _logger.LogInformation("Menu item {Name} updated successfully from restaurant {RestaurantId}.", item.Name, restaurantId);

            return _mapper.Map<UpdateMenuItemDto>(updated);
        }
        
        
        public async Task<IEnumerable<RestaurantDto>> GetMostRecentByUserAsync(int userId)
        {
            var restaurants = await _restaurantRepo.GetMostRecentByUserAsync(userId);

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            
        }
        public async Task<IEnumerable<RestaurantDto>> GetFavouriteByUserAsync(int userId)
        {
            var restaurants = await _restaurantRepo.GetFavouritesByUserAsync(userId);

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }
        public async Task<IEnumerable<RestaurantDto>> GetTopRatedAsync()
        {
            var restaurants = await _restaurantRepo.GetTopRatedAsync();

            return _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }




        private async Task<Restaurant> EnsureRestaurantExistsAsync(int id)
        {
            var restaurant = await _restaurantRepo.GetByIdAsync(id);

            return restaurant ?? throw new NotFoundException("Restaurant", id);
        }
    }
}
