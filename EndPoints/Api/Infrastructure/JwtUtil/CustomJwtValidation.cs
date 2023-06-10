using Api.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Infrastructure.JwtUtil;

public class CustomJwtValidation
{

    public CustomJwtValidation()
    {
    }

    public async Task Validate(TokenValidatedContext context)
    {
        var userId = context.Principal.GetUserId();
        var user = UsersDb.Users.FirstOrDefault(f => f.Id == userId);
        if (user == null)
        {
            context.Fail("User NotFound");

        }
        var jwtToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var token = user.Tokens.Any(f => f.JwtToken == jwtToken);
        if (token == false)
        {
            context.Fail("Token NotFound");
        }
    }
}