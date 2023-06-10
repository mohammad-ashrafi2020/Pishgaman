namespace WebApp.Infrastructure.Models;

public class UserDto
{
    public string Password { get; set; }
    public long Id { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public List<UserToken> Tokens { get; set; } = new();
}

public enum UserRole
{
    Admin = 1,
    Client = 2
}

public class UserToken
{
    public string JwtToken { get; set; }
}