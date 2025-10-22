using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Enums;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly UserRepository _repository;

        public AdminUsersController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminUserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();

                var dtos = users.Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Username = u.Username,     // vracamo stvarni Username iz baze
                    Email = u.Email,
                    Role = u.Role
                });

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<AdminUserDto>> PostUsers([FromBody] CreateUserDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                    return BadRequest(new { message = "Password should be at least 6 characters." });

                var passwordPattern = @"^(?=.*\d)(?=.*[^a-zA-Z0-9]).+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, passwordPattern))
                    return BadRequest(new { message = "Password must contain at least one number and one special character." });

                if (dto.Role != Role.Courier && dto.Role != Role.RestaurantOwner)
                    return BadRequest(new { message = "The role should be set to Courier or RestaurantOwner." });

                if (await _repository.ExistsByEmailAsync(dto.Email))
                    return Conflict(new { message = "Email already exists." });

                // napravi neki username (bez provere zauzetosti, jer repo nema metodu)
                var baseUsername = (dto.FirstName + "." + dto.LastName)
                    .ToLowerInvariant()
                    .Replace(' ', '-');

                // hesiraj lozinku
                var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Username = baseUsername,
                    Email = dto.Email,
                    PasswordHash = hash,
                    Role = dto.Role,
                    IsSuspended = false
                };

                var saved = await _repository.AddUserAsync(user);

                var result = new AdminUserDto
                {
                    Id = saved.Id,
                    Username = saved.Username,
                    Email = saved.Email,
                    Role = saved.Role
                };

                return Created($"/api/admin/users/{saved.Id}", result);
            }
            catch (DbUpdateException ex)
            {
                // Pomogne pri debugu constraints-a
                return StatusCode(400, new { message = "DB error", detail = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("owners")]
        public async Task<ActionResult<List<OwnerDto>>> GetOwners()
        {
            var owners = await _repository.GetOwnersAsync();
            return Ok(owners);
        }
    }
}
