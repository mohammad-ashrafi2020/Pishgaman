using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Infrastructure.ActionFilters;

public class UserRequestActionFilter : ActionFilterAttribute
{
    private readonly IConfiguration _configuration;
    public UserRequestActionFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var safeHost = _configuration.GetSection("safeHost").Get<string>();
        var safeIp = _configuration.GetSection("safeIp").Get<string>();

        if (context.HttpContext.Request.Host.Host != safeHost)
        {
            context.Result = new JsonResult(new ApiResult()
            {
                MetaData = new MetaData()
                {
                    AppStatusCode = AppStatusCode.ServerError,
                    Message = "Invalid Host"
                },
                IsSuccess = false
            });
        }
        else if (context.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString() != safeIp)
        {
            context.Result = new JsonResult(new ApiResult()
            {
                MetaData = new MetaData()
                {
                    AppStatusCode = AppStatusCode.ServerError,
                    Message = "Invalid Ip"
                },
                IsSuccess = false
            });

        }
        else
        {
            await next();
        }
    }
}