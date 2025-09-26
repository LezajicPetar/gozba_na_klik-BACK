using gozba_na_klik.Model;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gozba_na_klik.Controllers
{
    public class AuthContoller : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthContoller(AuthService authService)
        {
            _authService = authService;
        }

        //api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginAsync(string email, string password)
        {
            var user = await _authService.LoginAsync(email, password);

            if (user == null) return Unauthorized("Email or password are incorrect");

            return Ok(user);
        }

        //api/auth/logout
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            await _authService.LogoutAsync();
            return Ok();
        }
    }
}
