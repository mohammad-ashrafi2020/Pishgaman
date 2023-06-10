using Newtonsoft.Json;
using System.Net;

namespace Api.Infrastructure.Middlewares;

public static class CheckRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseCheckRequestData(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckRequestMiddleware>();
    }
}
public class CheckRequestMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly ILogger<ApiCustomExceptionHandlerMiddleware> _logger;

    public CheckRequestMiddleware(RequestDelegate next,
        IHostEnvironment env,
        ILogger<ApiCustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var safeIps = new List<string>()
        {
            "0.0.0.0","::1"
        };

        //Agar Server Side Darkhast Ersal Beshe Ip Server Inja Daryaft Mishe Agar Client Side Bashe , Ip Client e
        var userIp = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();

        if (safeIps.Any(f => f == userIp) == false)
        {
            await WriteToResponseAsync("آیپی شما  در سیستم مجاز نیست");
        }

        var s = context.Request.PathBase;
        if (context.Request.Host.Host != "localhost")
        {
            await WriteToResponseAsync("سرویس درخواست دهنده نامعتبر است");
        }
        await _next(context);


        async Task WriteToResponseAsync(string message)
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

            var result = new ApiResult()
            {
                IsSuccess = false,
                MetaData = new()
                {
                    AppStatusCode = AppStatusCode.ServerError,
                    Message = message
                }
            };
            var json = JsonConvert.SerializeObject(result);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}