using gozba_na_klik.Dtos.Restaurants;
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
    }
}
