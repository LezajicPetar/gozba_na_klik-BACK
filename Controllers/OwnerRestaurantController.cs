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
        private readonly OwnerRestaurantService _service;


        private static readonly string[] Allowed = new[] { "image/jpeg", "image/png" };

        public OwnerRestaurantController(OwnerRestaurantService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantSummaryDto>>> MyRestaurants([FromQuery] int ownerId)
        {
            var data = await _service.GetMineAsync(ownerId);
            return Ok(data);
        }


        [HttpPut("~/api/restaurants/{id:int}/general")]
        public async Task<ActionResult<RestaurantDetailsDto>> UpdateGeneral(
            int id, [FromQuery] int ownerId, [FromBody] RestaurantUpsertDto dto)
                {
                    var saved = await _service.UpdateGeneralAsync(id, ownerId, dto);
                    return Ok(saved);
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
        public async Task<ActionResult<IEnumerable<RestaurantWorkTime>>> GetSchedule(int id, [FromQuery] int ownerId)
            => Ok(await _service.GetScheduleAsync(id, ownerId));

        [HttpPut("~/api/restaurants/{id:int}/schedule")]
        public async Task<IActionResult> SetSchedule(int id, [FromQuery] int ownerId, [FromBody] IEnumerable<RestaurantWorkTimeDto> times)
        {
            await _service.SetScheduleAsync(id, ownerId, times);
            return NoContent();
        }


        [HttpGet("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<IEnumerable<RestaurantExceptionDate>>> GetExceptions(int id, [FromQuery] int ownerId)
    => Ok(await _service.GetExceptionsAsync(id, ownerId));

        [HttpPost("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<RestaurantExceptionDate>> AddException(int id, [FromQuery] int ownerId, [FromBody] RestaurantExceptionDto dto)
            => Ok(await _service.AddExceptionAsync(id, ownerId, dto));

        [HttpDelete("~/api/restaurants/{id:int}/exceptions/{exId:int}")]
        public async Task<IActionResult> DeleteException(int id, int exId, [FromQuery] int ownerId)
            => (await _service.DeleteExceptionAsync(id, ownerId, exId)) ? NoContent() : NotFound();
    }
}
