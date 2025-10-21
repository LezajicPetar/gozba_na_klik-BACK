using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/restaurants")]
    public class AdminRestaurantsController:ControllerBase
    {
        private readonly IRestaurantService _service;

        public AdminRestaurantsController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync()
        {
            var restaurants = await _service.GetAllAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetByIdAsync(int id)
        {
           var restaurant = await _service.GetByIdAsync(id);
            return Ok(restaurant);
        }

        
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateAsync(RestaurantInputDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult<RestaurantDto>> UpdateAsync(int id, RestaurantInputDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
