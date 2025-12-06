using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouriersController : ControllerBase
    {
        private readonly ICourierService _svc;
        public CouriersController(ICourierService svc) => _svc = svc;

        [HttpPost("{userId:int}/ensure")]
        public async Task<IActionResult> Ensure(int userId, [FromQuery] string? vehicleType, CancellationToken ct)
        {
            await _svc.EnsureCourierAsync(userId, vehicleType, ct);
            return NoContent();
        }

        [HttpPut("{userId:int}/schedule")]
        public async Task<IActionResult> UpsertSchedule(int userId, [FromBody] WeeklyScheduleUpsertRequestDto dto, CancellationToken ct)
        {
            await _svc.UpsertScheduleAsync(userId, dto, ct);
            return NoContent();
        }

        [HttpGet("{userId:int}/schedule")]
        public async Task<IActionResult> GetSchedule(int userId, CancellationToken ct)
            => Ok(await _svc.GetScheduleAsync(userId, ct));

        [HttpGet("{userId:int}/status")]
        public async Task<IActionResult> Status(int userId, CancellationToken ct)
            => Ok(await _svc.GetStatusNowAsync(userId, ct));

        [HttpPost("{userId:int}/suspend")]
        public async Task<IActionResult> Suspend(int userId, CancellationToken ct)
        {
            await _svc.SuspendAsync(userId, ct);
            return NoContent();
        }

        [HttpDelete("{userId:int}/suspend")]
        public async Task<IActionResult> Unsuspend(int userId, CancellationToken ct)
        {
            await _svc.UnsuspendAsync(userId, ct);
            return NoContent();
        }
    }
}
