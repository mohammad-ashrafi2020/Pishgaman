using Api.Infrastructure;
using Api.Infrastructure.JwtUtil;
using Api.Infrastructure.Models;
using Api.ViewModels;
using Application.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AuthController : ApiController
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [Authorize]
    [HttpGet]
    public ApiResult<UserDto> GetCurrentUser()
    {
        return QueryResult(UsersDb.Users.FirstOrDefault(f => f.Id == User.GetUserId()));
    }
    [HttpPost]
    public ApiResult<string?> Login(LoginViewModel loginViewModel)
    {
        var user = UsersDb.Users.FirstOrDefault(f => f.PhoneNumber == loginViewModel.PhoneNumber && f.Password == loginViewModel.Password);
        if (user == null)
        {
            return CommandResult(OperationResult<string>.Error("کاربری با مشخصات وارد شده یافت نشد"));
        }

        var token = JwtTokenBuilder.BuildToken(user, _configuration);
        user.Tokens.Add(new UserToken()
        {
            JwtToken = token
        });
        return CommandResult(OperationResult<string>.Success(token));
    }

    [HttpDelete]
    public ApiResult LogoutAllLogins(string phoneNumber)
    {
        var user = UsersDb.Users.FirstOrDefault(f => f.PhoneNumber == phoneNumber);
        if (user == null)
        {
            return CommandResult(OperationResult.Error("کاربری یافت شند"));
        }

        user.Tokens.Clear();
        return CommandResult(OperationResult.Success());
    }
}
