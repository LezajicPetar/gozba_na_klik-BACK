using gozba_na_klik.Dtos.Restaurants;
using gozba_na_klik.Model;
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
                return BadRequest("ownerId is required");

            var restaurants = await _repo.GetByOwnerAsync(ownerId);

            var dtos = restaurants.Select(r => new RestaurantDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Phone = r.Phone,
                Capacity = r.Capacity,
                Photo = r.Photo, 
                OwnerId = r.OwnerId,
                OwnerName = r.Owner != null ? (r.Owner.FirstName + " " + r.Owner.LastName) : ""
            });

            return Ok(dtos);
        }

        [HttpPut("~/api/restaurants/{id:int}/general")]
        public async Task<ActionResult> UpdateGeneral(int id, [FromBody] RestaurantGeneralUpdateDto dto, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;  // vrati 400/403/404 ako je bilo koji od tih slučajeva

            var entity = check.Value!;

            entity.Name = string.IsNullOrWhiteSpace(dto.Name) ? entity.Name : dto.Name.Trim();
            entity.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            entity.Phone = string.IsNullOrWhiteSpace(dto.Phone) ? null : dto.Phone.Trim();
            entity.Capacity = dto.Capacity;

            await _repo.UpdateAsync(entity);
            return Ok();
        }

        [HttpPost("~/api/restaurants/{id:int}/cover")]
        [RequestSizeLimit(10_000_000)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadCover(int id, IFormFile file, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            if (file == null || file.Length == 0) return BadRequest("Fajl je obavezan.");
            if (!Allowed.Contains(file.ContentType)) return BadRequest("Dozvoljeni format: .jpg, .png.");

            var entity = check.Value!;

            var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadDir = Path.Combine(root, "uploads", "restaurants", id.ToString());
            Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var name = $"cover_{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(uploadDir, name);

            using (var stream = System.IO.File.Create(full))
                await file.CopyToAsync(stream);

            if (!string.IsNullOrWhiteSpace(entity.Photo))
            {
                var oldPath = Path.Combine(root, entity.Photo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
            }

            var relUrl = $"/uploads/restaurants/{id}/{name}";
            entity.Photo = relUrl;

            await _repo.UpdateAsync(entity);
            return Ok(new { coverUrl = relUrl });
        }


        // GET /api/restaurants/{id}/schedule?ownerId=
        [HttpGet("~/api/restaurants/{id:int}/schedule")]
        public async Task<ActionResult<IEnumerable<object>>> GetSchedule(int id, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            var list = await _repo.GetWorkTimesAsync(id);

            var byDay = list.ToDictionary(x => x.DayOfWeek, x => x);
            var result = new List<object>();
            for (int d = 0; d < 7; d++)
            {
                if (byDay.TryGetValue(d, out var w))
                {
                    result.Add(new
                    {
                        dayOfWeek = d,
                        isClosed = w.IsClosed,
                        open = w.Open == TimeSpan.Zero ? "" : w.Open.ToString(@"hh\:mm"),
                        close = w.Close == TimeSpan.Zero ? "" : w.Close.ToString(@"hh\:mm")
                    });
                }
                else
                {
                    result.Add(new { dayOfWeek = d, isClosed = true, open = "", close = "" });
                }
            }

            return Ok(result);
        }

        // PUT /api/restaurants/{id}/schedule?ownerId=
        [HttpPut("~/api/restaurants/{id:int}/schedule")]
        public async Task<ActionResult> PutSchedule(int id, [FromBody] List<RestaurantWorkTimeDto> dto, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            if (dto == null) return BadRequest("Body is required.");

            var map = dto?.ToDictionary(x => x.DayOfWeek) ?? new Dictionary<int, RestaurantWorkTimeDto>();
            var list = new List<RestaurantWorkTime>();

            for (int d = 0; d < 7; d++)
            {
                var item = map.ContainsKey(d) ? map[d] : new RestaurantWorkTimeDto { DayOfWeek = d, IsClosed = true };
                var wt = new RestaurantWorkTime
                {
                    Id = item.Id,
                    RestaurantId = id,
                    DayOfWeek = item.DayOfWeek,
                    IsClosed = item.IsClosed
                };

                if (item.IsClosed)
                {
                    wt.Open = TimeSpan.Zero;
                    wt.Close = TimeSpan.Zero;
                }
                else
                {
                    if (!TimeSpan.TryParse(item.Open, out var open)) open = TimeSpan.Zero;
                    if (!TimeSpan.TryParse(item.Close, out var close)) close = TimeSpan.Zero;
                    wt.Open = open;
                    wt.Close = close;
                }

                list.Add(wt);
            }

            await _repo.SetWorkTimesAsync(id, list);
            return Ok();
        }

        // GET /api/restaurants/{id}/exceptions?ownerId=
        [HttpGet("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<IEnumerable<RestaurantExceptionDate>>> GetExceptions(int id, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            var list = await _repo.GetExceptionsAsync(id);

            var dtos = list
                .OrderBy(e => e.Date)
                .Select(e => new RestaurantExceptionDto
                {
                    Id = e.Id,
                    Date = e.Date.ToString("yyyy-MM-dd"),
                    Reason = e.Reason
                });

            return Ok(dtos);
        }

        // POST /api/restaurants/{id}/exceptions?ownerId=
        [HttpPost("~/api/restaurants/{id:int}/exceptions")]
        public async Task<ActionResult<RestaurantExceptionDate>> AddException(int id, [FromBody] RestaurantExceptionInputDto dto, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            var dateUtc = DateTime.SpecifyKind(dto.Date.Date, DateTimeKind.Utc);

            var entity = new RestaurantExceptionDate
            {
                RestaurantId = id,
                Date = dateUtc,
                Reason = dto.Reason
            };

            var saved = await _repo.AddExceptionAsync(entity);

            var outDto = new RestaurantExceptionDto
            {
                Id = saved.Id,
                Date = saved.Date.ToString("yyyy-MM-dd"),
                Reason = saved.Reason
            };

            return Ok(outDto);
        }

        // DELETE /api/restaurants/{id}/exceptions/{exId}?ownerId=
        [HttpDelete("~/api/restaurants/{id:int}/exceptions/{exId:int}")]
        public async Task<ActionResult> DeleteException(int id, int exId, [FromQuery] int ownerId)
        {
            var check = await GetAuthorizedRestaurantEntity(id, ownerId);
            if (check.Result != null) return check.Result;

            var ok = await _repo.DeleteExceptionAsync(exId);
            return ok ? NoContent() : NotFound();
        }

        // helper: vraća Ok(restaurant) | NotFound | Forbid | BadRequest
        private async Task<ActionResult<Restaurant>> GetAuthorizedRestaurantEntity(int id, int ownerId)
        {
            if (ownerId <= 0) return BadRequest("ownerId is required.");
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return NotFound();
            if (r.OwnerId != ownerId) return Forbid();
            return r; // OK: vraća se Restaurant kroz ActionResult<T>
        }


    }
}
