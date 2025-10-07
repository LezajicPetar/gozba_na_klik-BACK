using Microsoft.AspNetCore.Mvc;
using gozba_na_klik.Data;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly GozbaDbContext _db;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] Allowed = new[] { "image/jpeg", "image/png" };
        
        public UsersController(GozbaDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        //POST /api/users/{id}/photo

        [HttpPost("{id:int}/photo")]
        [RequestSizeLimit(10_000_000)] //10 MB
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPhoto([FromRoute] int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Fajl je obavezan. ");
            if (!Allowed.Contains(file.ContentType)) return BadRequest("Dozvoljeni format: .jpg, .png ");
            
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            ///wwwroot/uploads/avatars
            var root = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            var uploadDir = Path.Combine(root, "uploads", "avatars");
            Directory.CreateDirectory(uploadDir);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var name = $"{id}_{Guid.NewGuid():N}{ext}";
            var full = Path.Combine(uploadDir, name);

            //sacuvan fajl

            using (var stream = System.IO.File.Create(full))
                await file.CopyToAsync(stream);


            // (opciono) obrisi staru sliku ako postoji
            if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            {
                var oldPath = Path.Combine(root, user.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            //Relativni url koji front moze da prikaze
            var relUrl = $"/uploads/avatars/{name}";
            user.ProfilePicture = relUrl;

            await _db.SaveChangesAsync();

            return Ok(new { avatarUrl = relUrl });
        }

        // DELETE /api/users/{id}/photo
        [HttpDelete("{id:int}/photo")]
        public async Task<IActionResult> DeletePhoto([FromRoute] int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            {
                var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var oldPath = Path.Combine(root, user.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            user.ProfilePicture = null;
            await _db.SaveChangesAsync();

            return NoContent();
        }

    }


}
