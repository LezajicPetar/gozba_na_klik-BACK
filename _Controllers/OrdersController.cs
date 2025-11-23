using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Service.Interfaces;
using gozba_na_klik.Service.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICourierService _courierService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IOrderService orderService,
            ICourierService courierService,
            ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _courierService = courierService;
            _logger = logger;
        }

        // Kreiranje porudzbine AZ
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);
            return Ok(order);
        }

        // Dobavljanje porudzbine AZ
        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int orderId, CancellationToken ct)
        {
            var order = await _orderService.GetByIdAsync(orderId, ct); // moraš imati ovu metodu u servisu
            return order is null ? NotFound() : Ok(order);
        }

        // Kurira - preuzima porudzbinu AZ
        [HttpPost("{orderId:int}/pickup")]
        public async Task<IActionResult> Pickup(int orderId, [FromQuery] int userId, CancellationToken ct)
        {
            await _courierService.StartDeliveryAsync(userId, orderId, ct);
            return NoContent();
        }

        // Kurir - dostavio porudzbinu AZ
        [HttpPost("{orderId:int}/delivered")]
        public async Task<IActionResult> Delivered(int orderId, [FromQuery] int userId, CancellationToken ct)
        {
            await _courierService.FinishedDeliveryAsync(userId, orderId, ct);
            return NoContent();
        }

        // Restoran / Employee - prihvata porudzbinu AZ
        [HttpPost("{orderId:int}/accept")]
        public async Task<IActionResult> Accept(int orderId)
        {
            await _orderService.AcceptAsync(orderId);
            return NoContent();
        }

        // Restoran / Employee - odbija porudzbinu AZ
        [HttpPost("{orderId:int}/reject")]
        public async Task<IActionResult> Reject(int orderId, [FromBody] RejectOrderDto? dto)
        {
            await _orderService.RejectAsync(orderId, dto);
            return NoContent();
        }
    }
}
