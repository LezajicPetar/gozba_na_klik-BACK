using gozba_na_klik.Dtos;
using gozba_na_klik.Dtos.Employee;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/restaurants/{restaurantId}/employees")]
    [Authorize(Roles = "RestaurantOwner")]
    public class EmployeeMenagementController : ControllerBase
    {
        private readonly IEmployeeMenagementService _service;

        public EmployeeMenagementController(IEmployeeMenagementService service)
        {
            _service = service;
        }

        // GET /api/restaurants/{restaurantId}/employees
        [HttpGet]
        public async Task<ActionResult<List<EmployeeDto>>> Get(int restaurantId)
            => Ok(await _service.GetByRestaurantIdAsync(restaurantId));

        // POST
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(int restaurantId, EmployeeCreateDto dto)
            => Ok(await _service.CreateAsync(restaurantId, dto));

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
                return NotFound("Zaposleni ne postoji.");

            return Ok(result);
        }


        // PATCH (toggle status)
        [HttpPatch("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var result = await _service.ToggleStatusAsync(id);

            if (result == null)
                return NotFound("Zaposleni ne postoji.");

            return Ok(result);
        }

    }
}

