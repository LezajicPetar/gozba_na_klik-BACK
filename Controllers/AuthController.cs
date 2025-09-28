using System;
using System.Threading.Tasks;
using BCrypt.Net;
using gozba_na_klik.Data;
using gozba_na_klik.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using gozba_na_klik.Services;
using System.Text.RegularExpressions;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GozbaDbContext _db;
    private readonly TokenService _tokenService;

    // Password politika
    private const int MinPasswordLen = 8;
    private const int MaxPasswordLen = 128;
    private const int MaxNameLen = 35;
    private const int MaxEmailLen = 255;

    // jednostavni regex-i
    private static readonly Regex _rxDigit = new("\\d", RegexOptions.Compiled);
    private static readonly Regex _rxSymbol = new("[^a-zA-Z0-9]", RegexOptions.Compiled);
    private static readonly Regex _rxEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);


    public AuthController(GozbaDbContext db, TokenService tokenService)
    {
        _db = db;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var first = (dto.FirstName ?? "").Trim();
        var last = (dto.LastName ?? "").Trim();
        var email = (dto.Email ?? "").Trim().ToLowerInvariant();
        var password = dto.Password ?? "";
        var cpass = dto.ConfirmPassword ?? "";

        // Validacija
        if (string.IsNullOrWhiteSpace(first) || first.Length > MaxNameLen)
            ModelState.AddModelError(nameof(dto.FirstName), $"Obavezno polje (max {MaxNameLen} karaktera).");

        if (string.IsNullOrWhiteSpace(last) || last.Length > MaxNameLen)
            ModelState.AddModelError(nameof(dto.LastName), $"Obavezno polje (max {MaxNameLen} karaktera).");

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
        var exists = await _db.Users.AnyAsync(u => u.Email == email);
        if (exists) return Conflict("Nalog sa tim email-om već postoji");

        // Kreiranje korisnika
        var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            FirstName = first,
            LastName = last,
            Email = email,
            PasswordHash = hash // Role ostaje default: Customer
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

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
}
