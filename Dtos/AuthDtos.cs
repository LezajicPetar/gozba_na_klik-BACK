public class RegisterDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}

public class UserDto
{
    public int Id { get; set; } 
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string? ProfilePicture { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public UserDto User { get; set; } = default!;
}
