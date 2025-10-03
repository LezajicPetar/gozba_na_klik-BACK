using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllergensController : ControllerBase
    {
        private readonly GozbaDbContext _dbContext;

        public AllergensController(GozbaDbContext db)
        {
            _dbContext = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var allergens = await _dbContext.Allergens
                .Select(a => new { a.Id, a.Name })
                .ToListAsync();

            return Ok(allergens);
        }
    }
}
