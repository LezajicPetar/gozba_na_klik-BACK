using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/restaurants")]
    [Authorize(Roles ="Admin")] //ogranicava pristup samo useru sa rolom admin
    public class AdminRestaurantsController : ControllerBase
    {
        private readonly IAdminRestaurantService _service;
        private readonly ILogger<AdminRestaurantsController> _logger;

        public AdminRestaurantsController(IAdminRestaurantService service, ILogger<AdminRestaurantsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllAsync()
        {
            _logger.LogInformation("GET /api/admin/restaurants started");
            var restaurants = await _service.GetAllAsync();
            _logger.LogInformation("GET /api/admin/restaurants returned {Count} items", restaurants.Count());
            return Ok(restaurants);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetByIdAsync(int id)
        {
            _logger.LogInformation("GET /api/admin/restaurants/{Id}", id);
            var restaurant = await _service.GetByIdAsync(id);
            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant {Id} not found", id);
                return NotFound();
            }
            return Ok(restaurant);
        }
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> CreateAsync([FromBody] RestaurantInputDto dto)
        {
            _logger.LogInformation("POST /api/admin/restaurants with OwnerId={OwnerId}", dto.OwnerId);
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RestaurantDto>> UpdateAsync(int id, [FromBody] RestaurantInputDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _repository.DeleteAsync(id);
                if (!success)
                    return NotFound(new { message = $"Restaurant with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}
