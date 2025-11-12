//using gozba_na_klik.Dtos.MenuItems;
//using gozba_na_klik.Dtos.Restaurants;
//using gozba_na_klik.Model;
//using gozba_na_klik.Service;
//using Microsoft.AspNetCore.Mvc;

//namespace gozba_na_klik.Controllers.OwnerArea
//{
//    [ApiController]
//    [Route("api/owner/restaurants")]
//    public class RestaurantOwnerController : ControllerBase
//    {
//        private readonly IRestaurantService _restaurantService;
//        private readonly IWebHostEnvironment _env;
//        private readonly ILogger<RestaurantOwnerController> _logger;

//        private static readonly string[] Allowed = new[] { "image/jpeg", "image/png" };

//        public RestaurantOwnerController(
//            IRestaurantService restaurantService,
//            IWebHostEnvironment env,
//            ILogger<RestaurantOwnerController> logger)
//        {
//            _restaurantService = restaurantService;
//            _env = env;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetAllByOwnerAsync([FromQuery] int ownerId)
//        {
//            _logger.LogInformation("HTTP GET /api/owner/restaurants triggered.");

//            var restaurants = await _restaurantService.GetAllByOwnerAsync(ownerId);

//            _logger.LogInformation("HTTP GET /api/owner/restaurants completed.");

//            return Ok(restaurants);
//        }

//        [HttpDelete("{restaurantId}/menu/{menuItemId}")]
//        public async Task<ActionResult> DeleteMenuItemAsync(int restaurantId, int menuItemId)
//        {
//            _logger.LogInformation("HTTP DELETE /api/owner/restaurants/{restaurantId}/menu/{menuItemId} triggered.", restaurantId, menuItemId);

//            await _restaurantService.DeleteMenuItemAsync(restaurantId, menuItemId);

//            _logger.LogInformation("HTTP DELETE /api/owner/restaurants/{restaurantId}/menu/{menuItemId} completed.", restaurantId, menuItemId);

//            return NoContent();
//        }

//        [HttpPut("{restaurantId}/menu/{menuItemId}")]
//        public async Task<ActionResult<UpdateMenuItemDto>> UpdateMenuItemAsync(int restaurantId, [FromBody] UpdateMenuItemDto item)
//        {
//            _logger.LogInformation("HTTP PUT /api/owner/restaurants/{restaurantId}/menu/{menuItemId} triggered.", restaurantId, item.Id);

//            var updated = await _restaurantService.UpdateMenuItemAsync(restaurantId, item);

//            _logger.LogInformation("HTTP PUT /api/owner/restaurants/{restaurantId}/menu/{menuItemId} completed.", restaurantId, item.Id);

//            return Ok(updated);
//        }

//    }
//}
//[HttpPut("{restaurantId}/menu/${menuItemId}")]
//public async Task<ActionResult<Restaurant>> UpdateMenuItem(
//    int restaurantId, 
//    int menuItemId, 
//    [FromBody]MenuItem item)
//{
//    if (menuItemId != menuItemDto.Id)
//        return BadRequest("ID u URL-u i telu zahteva se ne poklapaju.");
//}

//[HttpPut("~/api/restaurants/{id:int}/general")]
//public async Task<ActionResult> UpdateGeneral([FromRoute] int id, [FromBody] RestaurantGeneralUpdateDto dto, [FromQuery] int ownerId)
//{

//    var restaurant = await _repo.GetByIdAsync(id);

//    restaurant.Name = string.IsNullOrWhiteSpace(dto.Name) ? restaurant.Name : dto.Name.Trim();
//    restaurant.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
//    restaurant.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();
//    restaurant.Capacity = dto.Capacity;

//    var updated = await _repo.UpdateAsync(restaurant);
//    return Ok();
//}

//[HttpPost("~/api/restaurants/{id:int}/cover")]
//[RequestSizeLimit(10_000_000)]
//[Consumes("multipart/form-data")]
//public async Task<ActionResult> UploadCover([FromRoute] int id, IFormFile file, [FromQuery] int ownerId)
//{
//    if (ownerId <= 0)
//        return BadRequest("ownerId is required in DEV.");

//    if (file == null || file.Length == 0)
//        return BadRequest("Fajl je obavezan.");
//    if (!Allowed.Contains(file.ContentType))
//        return BadRequest("Dozvoljeni format: .jpg, .png.");

//    var restaurant = await _repo.GetByIdAsync(id);
//    if (restaurant == null) return NotFound();
//    if (restaurant.OwnerId != ownerId) return Forbid();

//    var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
//    var uploadDir = Path.Combine(root, "uploads", "restaurants", id.ToString());
//    Directory.CreateDirectory(uploadDir);

//    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
//    var name = $"cover_{Guid.NewGuid():N}{ext}";
//    var full = Path.Combine(uploadDir, name);

//    using (var stream = System.IO.File.Create(full))
//        await file.CopyToAsync(stream);

//    // obriši staru sliku ako postoji
//    if (!string.IsNullOrWhiteSpace(restaurant.Photo))
//    {
//        var oldPath = Path.Combine(root, restaurant.Photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
//        if (System.IO.File.Exists(oldPath))
//            System.IO.File.Delete(oldPath);
//    }

//    var relUrl = $"/uploads/restaurants/{id}/{name}";
//    restaurant.Photo = relUrl;

//    await _repo.UpdateAsync(restaurant);

//    return Ok(new { coverUrl = relUrl });
//}
//}

//}
