using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Enums;
using gozba_na_klik.Model;
using gozba_na_klik.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    //[Authorize(Roles = "Admin")] // odkomentariši kad dodaš auth
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
                    Username = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Role = u.Role
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

        [HttpPost]
        public async Task<ActionResult<AdminUserDto>> PostUsers(CreateUserDto dto)
        {
            try
            {
                if (dto.Password.Length < 6)
                    return BadRequest(new { message = "Password should be at least 6 characters." });

                var passwordPattern = @"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Password, passwordPattern))
                    return BadRequest(new { message = "Password must contain at least one number and one special character." });

                if (await _repository.ExistsByEmailAsync(dto.Email))
                    return BadRequest(new { message = "The email already exists." });

                if (await _repository.ExistsByNameAsync(dto.FirstName, dto.LastName))
                    return BadRequest(new { message = "User with this name already exists." });

                if (dto.Role != Role.Courier && dto.Role != Role.RestaurantOwner)
                    return BadRequest(new { message = "The role should be set to Courier or RestaurantOwner." });

                var user = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = dto.Password,
                    Role = dto.Role
                };

                var saved = await _repository.AddUserAsync(user);

                return Ok(new AdminUserDto
                {
                    Id = saved.Id,
                    Username = saved.FirstName.ToLower() + saved.LastName.ToLower(),
                    Email = saved.Email,
                    Role = saved.Role
                });
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

        //GET za vlasnike restorana

        [HttpGet("owners")]
        public async Task<ActionResult<List<OwnerDto>>> GetOwners()
        {
            var owners = await _repository.GetOwnersAsync();
            return Ok(owners);
        }

    }
}
