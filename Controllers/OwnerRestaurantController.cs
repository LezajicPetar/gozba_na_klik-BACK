using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Repository;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers.OwnerArea
{
    [ApiController]
    [Route("api/owner/restaurants")]
    public class OwnerRestaurantController : ControllerBase
    {
        private readonly RestaurantRepository _repo;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] Allowed = new[] { "image/jpeg", "image/png" };

        public OwnerRestaurantController(RestaurantRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantDto>>> MyRestaurants([FromQuery] int ownerId)
        {
            if (ownerId <= 0)
                return BadRequest("ownerId is required in DEV.");

            var restaurants = await _repo.GetByOwnerAsync(ownerId);

            var dtos = restaurants.Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Photo = r.Photo, 
                OwnerId = r.OwnerId,
                OwnerName = r.Owner != null ? (r.Owner.FirstName + " " + r.Owner.LastName) : ""
            });

            return Ok(dtos);
        }

        [HttpPut("~/api/restaurants/{id:int}/general")]
        public async Task<ActionResult> UpdateGeneral([FromRoute] int id, [FromBody] RestaurantGeneralUpdateDto dto, [FromQuery] int ownerId)
        {
            if (ownerId <= 0)
                return BadRequest("ownerId is required in DEV.");

            var restaurant = await _repo.GetByIdAsync(id);
            if (restaurant == null) return NotFound();
            if (restaurant.OwnerId != ownerId) return Forbid();

            restaurant.Name = string.IsNullOrWhiteSpace(dto.Name) ? restaurant.Name : dto.Name.Trim();
            restaurant.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            restaurant.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();
            restaurant.Capacity = dto.Capacity;

            await _repo.UpdateAsync(restaurant);
            return Ok();
        }

        [HttpPost("~/api/restaurants/{id:int}/cover")]
        [RequestSizeLimit(10_000_000)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadCover([FromRoute] int id, IFormFile file, [FromQuery] int ownerId)
        {
            if (ownerId <= 0)
                return BadRequest("ownerId is required in DEV.");

            if (file == null || file.Length == 0)
                return BadRequest("Fajl je obavezan.");
            if (!Allowed.Contains(file.ContentType))
                return BadRequest("Dozvoljeni format: .jpg, .png.");

            var restaurant = await _repo.GetByIdAsync(id);
            if (restaurant == null) return NotFound();
            if (restaurant.OwnerId != ownerId) return Forbid();

            var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadDir = Path.Combine(root, "uploads", "restaurants", id.ToString());
            Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var name = $"cover_{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(uploadDir, name);

            using (var stream = System.IO.File.Create(full))
                await file.CopyToAsync(stream);

            // obriši staru sliku ako postoji
            if (!string.IsNullOrWhiteSpace(restaurant.Photo))
            {
                var oldPath = Path.Combine(root, restaurant.Photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var relUrl = $"/uploads/restaurants/{id}/{name}";
            restaurant.Photo = relUrl;

            await _repo.UpdateAsync(restaurant);

            return Ok(new { coverUrl = relUrl });
        }
    }

    public class RestaurantGeneralUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public int? Capacity { get; set; }
    }
}
