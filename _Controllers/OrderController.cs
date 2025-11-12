using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/customer/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateAsync([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateAsync(dto);

            return Ok(order);
        }
    }
}
