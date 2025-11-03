using gozba_na_klik.Dtos;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId}/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;

        public AddressController(AddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(int customerId)
        {
            var addresses = await _addressService.GetByUserIdAsync(customerId);
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int customerId, int id)
        {
            var address = await _addressService.GetByIdAsync(id, customerId);
            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(int customerId, [FromBody] AddressInputDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var address = await _addressService.CreateAsync(dto, customerId);
            return CreatedAtAction(nameof(GetAddress), new { customerId, id = address.Id }, address);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int customerId, int id, [FromBody] AddressUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var address = await _addressService.UpdateAsync(id, dto, customerId);
            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int customerId, int id)
        {
            var deleted = await _addressService.DeleteAsync(id, customerId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
