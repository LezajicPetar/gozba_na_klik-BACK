using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik._Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public RestaurantOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Zaposleni ili vlasnik prihvata porudzbinu AZ
        [HttpPost("{orderId:int}/accept")]
        public async Task<IActionResult> Accept(int orderId)
        {
            await _orderService.AcceptAsync(orderId);
            return NoContent();
        }

        // Zaposleni ili vlasnik odbija porudzbinu AZ
        [HttpPost("{orderId:int}/reject")]
        public async Task<IActionResult> Reject(int orderId, [FromBody] RejectOrderDto dto)
        {
            await _orderService.RejectAsync(orderId, dto);
            return NoContent();
        }
    }
}
