using WebApp.Infrastructure;
using WebApp.Infrastructure.Models;

namespace WebApp.Services.Auth;

public interface IAuthService
{
    Task<ApiResult<UserDto>> GetCurrentUser();
    Task<ApiResult<string>> Login(string phoneNumber, string password);
}

public class AuthService : IAuthService
{
    private readonly HttpClient _client;
    private IHttpContextAccessor _accessor;
    public AuthService(HttpClient client, IHttpContextAccessor accessor)
    {
        _client = client;
        _accessor = accessor;
    }

    public async Task<ApiResult<UserDto>> GetCurrentUser()
    {
        return await _client.GetFromJsonAsync<ApiResult<UserDto>>("auth");
    }

    public async Task<ApiResult<string>> Login(string phoneNumber, string password)
    {
        var command = new
        {
            phoneNumber,
            password
        };
        var result = await _client.PostAsJsonAsync("auth", command);
        return await result.Content.ReadFromJsonAsync<ApiResult<string>>();
    }
}