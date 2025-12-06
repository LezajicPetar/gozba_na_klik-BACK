using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Dtos.Users;
using gozba_na_klik.Service.Interfaces;
using gozba_na_klik.Service.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetForCustomer([FromQuery] int customerId)
        {
            if (customerId <= 0)
                return BadRequest("customer is required.");

            _logger.LogInformation("Fetching orders for customer with ID {CustomerId}", customerId);

            var list = await _orderService.GetByCustomerAsync(customerId);
            return Ok(list);

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
        [Authorize(Roles = "Employee, RestaurantOwner")]
        public async Task<IActionResult> Accept(int orderId)
        {
            await _orderService.AcceptAsync(orderId);
            return NoContent();
        }

        // Restoran / Employee - odbija porudzbinu AZ
        [HttpPost("{orderId:int}/reject")]
        [Authorize(Roles = "Employee, RestaurantOwner")]
        public async Task<IActionResult> Reject(int orderId, [FromBody] RejectOrderDto? dto)
        {
            await _orderService.RejectAsync(orderId, dto);
            return NoContent();
        }

        // Restoran / Employee - dobavljanje svih na cekanju AZ
        [HttpGet("pending")]
        [Authorize(Roles = "Employee, RestaurantOwner")]
        public async Task<ActionResult<List<OrderDto>>> GetPending(CancellationToken ct = default)
        {
            _logger.LogInformation("Fetching pending orders for employees.");
            var orders = await _orderService.GetPendingAsync(ct);
            return Ok(orders);
        }

        // Vlasnik restorana - dobavljanje svih na cekanju za njegove restorane AZ
        [HttpGet("pending/owner/{ownerId:int}")]
        [Authorize(Roles = "RestaurantOwner")]
        public async Task<ActionResult<List<OrderDto>>> GetPendingForOwner(int ownerId, CancellationToken ct = default)
        {
            _logger.LogInformation("Fetching pending orders for restaurant owner with ID {OwnerId}", ownerId);
            var orders = await _orderService.GetPendingForOwnerAsync(ownerId, ct);
            return Ok(orders);
        }

        [HttpPut("{orderId:int}/assign")]
        [Authorize(Roles = "Employee, RestaurantOwner")]
        public async Task<IActionResult> AssignCourier(int orderId, [FromQuery] int courierId)
        {
            await _orderService.AssignCourierAsync(orderId, courierId);
            return NoContent();
        }

    }
}
