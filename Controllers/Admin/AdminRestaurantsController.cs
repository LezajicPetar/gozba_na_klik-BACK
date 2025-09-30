using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/restaurants")]
    public class AdminRestaurantsController:ControllerBase
    {
        private readonly RestaurantRepository _repository;

        public AdminRestaurantsController(RestaurantRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAll()
        {
            try
            {
                var restaurants = await _repository.GetAllAsync();

                var dtos = restaurants.Select(r => new RestaurantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    OwnerId = r.OwnerId,
                    OwnerName = r.Owner.FirstName + " " + r.Owner.LastName
                });

                return Ok(dtos);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = "Invalid request.", detail = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "A database error occurred.", detail = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

    }
}

