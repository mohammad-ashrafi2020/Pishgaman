﻿using System.Net;
using Newtonsoft.Json;

namespace Api.Infrastructure.Middlewares;

public static class ApiCustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseApiCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiCustomExceptionHandlerMiddleware>();
    }
}

public class ApiCustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly ILogger<ApiCustomExceptionHandlerMiddleware> _logger;

    public ApiCustomExceptionHandlerMiddleware(RequestDelegate next,
        IHostEnvironment env,
        ILogger<ApiCustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        string message = null;
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        AppStatusCode apiStatusCode = AppStatusCode.ServerError;

        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            SetErrorMessage(exception);
            await WriteToResponseAsync();
        }

        void SetErrorMessage(Exception exception)
        {
            message = exception.Message;
            if (_env.IsDevelopment())
            {
                var dic = new Dictionary<string, string>
                {
                    ["Exception"] = exception.Message,
                    ["StackTrace"] = exception.StackTrace,
                };
                if (exception.InnerException != null)
                {
                    dic.Add("InnerException.Exception", exception.InnerException.Message);
                    dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                }

                message = JsonConvert.SerializeObject(dic);
            }
        }
        async Task WriteToResponseAsync()
        {
            if (context.Response.HasStarted)
                throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");

            var result = new ApiResult()
            {
                IsSuccess = false,
                MetaData = new()
                {
                    AppStatusCode = apiStatusCode,
                    Message = message
                }
            };
            var json = JsonConvert.SerializeObject(result);

            context.Response.StatusCode = (int)httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}