using gozba_na_klik.Dtos.MenuItem;
using gozba_na_klik.Dtos.MenuItems;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers.OwnerArea
{
    [ApiController]
    [Route("api/owner/restaurants")]
    public class OwnerRestaurantController : ControllerBase
    {
        private readonly IOwnerRestaurantService _service;
        private readonly ILogger<OwnerRestaurantController> _logger;

        public OwnerRestaurantController(IOwnerRestaurantService service, ILogger<OwnerRestaurantController> logger)
        {
            _service = service;
            _logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantSummaryDto>>> MyRestaurants([FromQuery] int ownerId)
        {
            var data = await _service.GetMineAsync(ownerId);
            return Ok(data);
        }


        [HttpPut("{restaurantId}/menu/{menuItemId}")]
        public async Task<ActionResult<ReadMenuItemDto>> UpdateMenuItemAsync(int restaurantId, [FromBody] UpdateMenuItemDto item)
        {
            _logger.LogInformation("HTTP PUT /api/owner/restaurants/{restaurantId}/menu/{menuItemId} triggered.", restaurantId, item.Id);

            var updated = await _service.UpdateMenuItemAsync(restaurantId, item);

            _logger.LogInformation("HTTP PUT /api/owner/restaurants/{restaurantId}/menu/{menuItemId} completed.", restaurantId, item.Id);

            return Ok(updated);
        }
        
        [HttpDelete("{restaurantId}/menu/{menuItemId}")]
        public async Task<ActionResult> DeleteMenuItemAsync(int restaurantId, int menuItemId)
        {
            _logger.LogInformation("HTTP DELETE /api/owner/restaurants/{restaurantId}/menu/{menuItemId} triggered.", restaurantId, menuItemId);

            await _service.DeleteMenuItemAsync(restaurantId, menuItemId);

            _logger.LogInformation("HTTP DELETE /api/owner/restaurants/{restaurantId}/menu/{menuItemId} completed.", restaurantId, menuItemId);

            return NoContent();
        }
        
        [HttpPost("{restaurantId}/menu")]
        public async Task<ActionResult<ReadMenuItemDto>> CreateMenuItemAsync(int restaurantId, CreateMenuItemDto dto)
        {
            _logger.LogInformation("HTTP POST /api/owner/restaurants/{restaurantId}/menu triggered.", restaurantId);

            var menuItem = await _service.CreateMenuItemAsync(restaurantId, dto);

            _logger.LogInformation("HTTP POST /api/owner/restaurants/{restaurantId}/menu completed.", restaurantId);

            return Ok(menuItem);
        }




        [HttpPut("~/api/restaurants/{id:int}/general")]
        public async Task<ActionResult<RestaurantDetailsDto>> UpdateGeneral(
            int id, [FromQuery] int ownerId, [FromBody] RestaurantUpsertDto dto)
        {
            var updated = await _service.UpdateGeneralAsync(id, ownerId, dto);
            return Ok(updated);
        }

        [HttpPost("~/api/restaurants/{id:int}/cover")]
        [RequestSizeLimit(10_000_000)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<object>> UploadCover(int id, [FromQuery] int ownerId, IFormFile file)
        {
            var url = await _service.UpdateCoverAsync(id, ownerId, file);
            return Ok(new { coverUrl = url });
        }

        [HttpGet("~/api/restaurants/{id:int}/schedule")]
        public async Task<ActionResult<IEnumerable<RestaurantWorkTimeDto>>> GetSchedule(int id, [FromQuery] int ownerId)
        {
            var times = await _service.GetScheduleAsync(id, ownerId);
            return Ok(times);
        }

        [HttpPut("~/api/restaurants/{id:int}/schedule")]
        public async Task<IActionResult> SetSchedule(int id, [FromQuery] int ownerId, [FromBody] IEnumerable<RestaurantWorkTimeDto> times)
        {
            await _service.SetScheduleAsync(id, ownerId, times);
            return NoContent();
        }

        [HttpGet("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<IEnumerable<RestaurantExceptionDto>>> GetExceptions(int id, [FromQuery] int ownerId)
        {
            var ex = await _service.GetExceptionsAsync(id, ownerId);
            return Ok(ex);
        }

        [HttpPost("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<RestaurantExceptionDto>> AddException(int id, [FromQuery] int ownerId, [FromBody] RestaurantExceptionDto dto)
        {
            var added = await _service.AddExceptionAsync(id, ownerId, dto);
            return Ok(added);
        }

        [HttpDelete("~/api/restaurants/{id:int}/exceptions/{exId:int}")]
        public async Task<IActionResult> DeleteException(int id, int exId, [FromQuery] int ownerId)
        {
            var deleted = await _service.DeleteExceptionAsync(id, ownerId, exId);
            return deleted ? NoContent() : NotFound();
        }
    }
}
