using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCrypt.Net;
using gozba_na_klik.Data;
using gozba_na_klik.Dtos;
using gozba_na_klik.DtosAdmin;
using gozba_na_klik.Model.Entities;
using gozba_na_klik.Service.External;
using gozba_na_klik.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;



[ApiController]
[Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
    private readonly TokenService _tokenService;
    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;

    // Password politika
    private const int MinPasswordLen = 8;
    private const int MaxPasswordLen = 128;
    private const int MaxNameLen = 35;
    private const int MaxEmailLen = 255;
    private const int MaxUsernameLen = 12;

    // jednostavni regex-i
    private static readonly Regex _rxDigit = new("\\d", RegexOptions.Compiled);
    private static readonly Regex _rxSymbol = new("[^a-zA-Z0-9]", RegexOptions.Compiled);
    private static readonly Regex _rxEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);


    public AuthController(TokenService tokenService, AuthService authService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _authService = authService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> LoginAsync([FromBody] LoginDto dto)
    {
        var auth = await _authService.LoginAsync(dto);
        return auth is null ? Unauthorized() : Ok(auth);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        return Ok();
    }


    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> RegisterAsync([FromBody] RegisterDto dto)
    {
        var first = (dto.FirstName ?? "").Trim();
        var last = (dto.LastName ?? "").Trim();
        var email = (dto.Email ?? "").Trim().ToLowerInvariant();
        var password = dto.Password ?? "";
        var username = (dto.Username ?? "").Trim();
        var cpass = dto.ConfirmPassword ?? "";

        // Validacija
        if (string.IsNullOrWhiteSpace(first) || first.Length > MaxNameLen)
            ModelState.AddModelError(nameof(dto.FirstName), $"Obavezno polje (max {MaxNameLen} karaktera).");

        if (string.IsNullOrWhiteSpace(last) || last.Length > MaxNameLen)
            ModelState.AddModelError(nameof(dto.LastName), $"Obavezno polje (max {MaxNameLen} karaktera).");

        if (string.IsNullOrWhiteSpace(username) || username.Length > MaxUsernameLen)
            ModelState.AddModelError(nameof(dto.Username), $"Obavezno polje (max {MaxUsernameLen} karaktera).");

        if (string.IsNullOrWhiteSpace(email) || email.Length > MaxEmailLen ||! _rxEmail.IsMatch(email))
            ModelState.AddModelError(nameof(dto.Email), "Email nije validan.");

        if (password.Length < MinPasswordLen || password.Length > MaxPasswordLen)
            ModelState.AddModelError(nameof(dto.Password), $"Lozinka mora imati između {MinPasswordLen} i {MaxPasswordLen} karaktera.");

        if (!_rxDigit.IsMatch(password)) ModelState.AddModelError(nameof(dto.Password), "Mora sadržati bar jednu cifru.");
        if (!_rxSymbol.IsMatch(password)) ModelState.AddModelError(nameof(dto.Password), "Mora sadržati bar jedan specijalni karakter.");

        if (password != cpass)
            ModelState.AddModelError(nameof(dto.ConfirmPassword), "Lozinke se ne poklapaju.");

        // Ako ima ijedna greska - vrati 400 sa detaljima
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        // Unikatnost email-a 
        var exists = await _authService.GetByEmailAsync(email);
        if (exists != null) return Conflict("Nalog sa tim email-om već postoji");

        // Kreiranje korisnika
        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            FirstName = first,
            LastName = last,
            Email = email,
            Username = username,
            PasswordHash = hash // Role ostaje default: Customer
        };

        user = await _authService.RegisterUserAsync(user);

        // Token + odgovor
        var token = _tokenService.Generate(user);

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role.ToString(),
            ProfilePicture = user.ProfilePicture
        };

        return Ok(new AuthResponseDto { Token = token, User = userDto });
    }

    [HttpGet("admin-token")]
    public IActionResult GetAdminToken()
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Email, "admin@gozba.com"),
        new Claim(ClaimTypes.Role, "Admin")
    };

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { token = tokenString });
    }

}
