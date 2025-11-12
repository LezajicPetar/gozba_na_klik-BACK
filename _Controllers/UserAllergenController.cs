using gozba_na_klik.Services;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api")]

    public class UserAllergenController : Controller
    {
        private readonly UserAllergenService _userAllergenService;

        public UserAllergenController(UserAllergenService userAllergenService)
        {
            _userAllergenService = userAllergenService;
        }

        [HttpGet("users/{id}/allergens")]
        public async Task<IActionResult> GetAllByUser(int id)
        {
            var allergens = await _userAllergenService.GetAllByUserAsync(id);

            return allergens is null ? NoContent() : Ok(allergens);
        }

    }
}
