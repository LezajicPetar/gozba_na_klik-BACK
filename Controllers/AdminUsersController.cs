using gozba_na_klik.Data;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Enums;
using gozba_na_klik.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers
{
    [Route("api/adminusers")]
    [ApiController]
    //[Authorize(Roles = "Admin")] ovo odkomentarisati kasnije 
    public class AdminUsersController : ControllerBase
    {
        private readonly GozbaDbContext _context;

        public AdminUsersController(GozbaDbContext context)
        {
            _context = context;
        }

        //GET api/adminusers
        [HttpGet]
        public ActionResult<IEnumerable<AdminUserDto>> GetUsers()
        {
            var users= _context.Users
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    Username = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Role = u.Role
                })
                .ToList();

            return Ok(users);
        }

        //POST
        [HttpPost]
        public ActionResult<AdminUserDto> PostUsers(CreateUserDto dto) {
            if (dto.Password.Length < 6)
            {
                return BadRequest("Password should be at least 6 characters.");
            }

            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return BadRequest("The email already exist.");
            }

            if (_context.Users.Any(u => u.FirstName == dto.FirstName && u.LastName == dto.LastName))
                return BadRequest("User with this name already exists.");

            if (dto.Role != Role.Courier && dto.Role != Role.RestaurantOwner)
            {
                return BadRequest("The role should be set to Courier or Restaurant owner.");
            }

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new AdminUserDto
            {
                Id = user.Id,
                Username = user.FirstName.ToLower() + user.LastName.ToLower(),
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}
