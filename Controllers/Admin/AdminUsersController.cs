using gozba_na_klik.Dtos.Users;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gozba_na_klik.Controllers.Admin
{
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _service;
        private readonly ILogger<AdminUsersController> _logger;


        public AdminUsersController(IAdminUserService service, ILogger<AdminUsersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminUserDto>>> GetAllAsync()
        {
            _logger.LogInformation("GET /api/admin/users");
            var users = await _service.GetAllAsync();
            _logger.LogInformation("Returned {Count} users", users.Count());
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<AdminUserDto>> CreateAsync([FromBody] CreateUserDto dto)
        {
            _logger.LogInformation("POST /api/admin/users with Email={Email}", dto.Email);
            var createdUser = await _service.CreateAsync(dto);
            _logger.LogInformation("Created user {Id}", createdUser.Id);
            return Created("", createdUser);
        }

        //GET za vlasnike restorana

        [HttpGet("owners")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetOwnersAsync()
        {
            _logger.LogInformation("GET /api/admin/users/owners");
            var owners = await _service.GetOwnersAsync();
            _logger.LogInformation("Returned {Count} restaurant owners", owners.Count());
            return Ok(owners);
        }
    }
}