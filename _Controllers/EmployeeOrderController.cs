using gozba_na_klik.Dtos.Order;
using gozba_na_klik.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeOrderController : ControllerBase
    {
        private readonly IEmployeeOrderService _employeeOrders;

        public EmployeeOrderController(IEmployeeOrderService employeeOrders)
        {
            _employeeOrders = employeeOrders;
        }

        // GET: api/employees/{employeeId}/orders/pending
        [HttpGet("{employeeId:int}/orders/pending")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<EmployeeOrderListItemDto>>> GetPendingOrders(
            int employeeId,
            CancellationToken ct)
        {
            var orders = await _employeeOrders.GetPendingOrdersForEmployeeAsync(employeeId, ct);
            return Ok(orders);
        }

        // GET: api/employees/me/orders/pending
        [HttpGet("me/orders/pending")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<EmployeeOrderListItemDto>>> GetMyPendingOrders(
            CancellationToken ct)
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == "nameid" || c.Type.EndsWith("/nameidentifier"));
            if (idClaim == null || !int.TryParse(idClaim.Value, out var userId))
            {
                return Unauthorized();
            }

            var orders = await _employeeOrders.GetPendingOrdersForEmployeeAsync(userId, ct);
            return Ok(orders);
        }
    }
}
