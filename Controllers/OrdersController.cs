using gozba_na_klik.Service.Implementations;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICourierService _courierSvc;
        public OrdersController(ICourierService courierSvc) => _courierSvc = courierSvc;

        [HttpPost("{orderId:int}/pickup")]
        public async Task<IActionResult> Pickup (int orderId, [FromQuery]int userId, CancellationToken ct)
        {
            await _courierSvc.StartDeliveryAsync(userId, orderId, ct);
            return NoContent();
        }

        [HttpPost("{orderId:int}/delivered")]
        public async Task<IActionResult> Delivered(int orderId, [FromQuery]int userId, CancellationToken ct)
        {
            await _courierSvc.FinishedDeliveryAsync(userId, orderId, ct);
            return NoContent();
        }
    }
}
