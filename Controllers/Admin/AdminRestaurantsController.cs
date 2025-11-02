using gozba_na_klik.Data;
using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetById(int id)
        {
            try
            {
                var restaurant = await _repository.GetByIdAsync(id);
                if (restaurant == null)
                    return NotFound(new { message = $"Restaurant with ID {id} was not found." });

                var dto = new RestaurantDto
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    OwnerId = restaurant.OwnerId,
                    OwnerName = restaurant.Owner.FirstName + " " + restaurant.Owner.LastName,
                    Photo = restaurant.Photo
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<RestaurantDto>> Create(RestaurantInputDto dto)
        {
            try
            {
                var restaurant = new Restaurant
                {
                    Name = dto.Name,
                    OwnerId = dto.OwnerId,
                    Photo = dto.Photo
                };

                var saved = await _repository.AddAsync(restaurant);
                var withOwner = await _repository.GetByIdAsync(saved.Id);

                var result = new RestaurantDto
                {
                    Id = saved.Id,
                    Name = saved.Name,
                    OwnerId = saved.OwnerId,
                    OwnerName = saved.Owner.FirstName + " " + saved.Owner.LastName,
                    Photo = saved.Photo
                };

                return Ok(result);
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

        
        [HttpPut("{id}")]
        public async Task<ActionResult<RestaurantDto>> Update(int id, RestaurantInputDto dto)
        {
            try
            {
                var restaurant = await _repository.GetByIdAsync(id);
                if (restaurant == null)
                    return NotFound(new { message = $"Restaurant with ID {id} was not found." });

                restaurant.Name = dto.Name;
                restaurant.OwnerId = dto.OwnerId;
                restaurant.Photo = dto.Photo;

                var updated = await _repository.UpdateAsync(restaurant);
                var withOwner = await _repository.GetByIdAsync(id);

                var result = new RestaurantDto
                {
                    Id = updated.Id,
                    Name = updated.Name,
                    OwnerId = updated.OwnerId,
                    OwnerName = updated.Owner.FirstName + " " + updated.Owner.LastName,
                    Photo = updated.Photo
                };

                return Ok(result);
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

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var restaurant = await _repository.DeleteAsync(id);
                if (restaurant == null)
                    return NotFound(new { message = $"Restaurant with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }
    }
}
