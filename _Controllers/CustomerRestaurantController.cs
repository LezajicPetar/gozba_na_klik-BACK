using gozba_na_klik.Dtos.Queries;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/customer/restaurants")]
    public class CustomerRestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;


        public CustomerRestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync()
        {
            var restaurants = await _restaurantService.GetAllAsync();

            return Ok(restaurants);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedRestaurantsAsync([FromQuery] RestaurantQuery query)
        {
            var result = await _restaurantService.GetPagedRestaurantsAsync(query);

            return Ok(result);
        }

        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetMostRecentByUserAsync(int userId)
        {
            var restaurants = await _restaurantService.GetMostRecentByUserAsync(userId);

            return Ok(restaurants);
        }
        [HttpGet("favourites")]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetFavouriteByUserAsync(int userId)
        {
            var restaurants = await _restaurantService.GetFavouriteByUserAsync(userId);

            return Ok(restaurants);
        }
        [HttpGet("top-rated")]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetTopRatedAsync()
        {
            var restaurants = await _restaurantService.GetTopRatedAsync();

            return Ok(restaurants);
        }

    }
}
