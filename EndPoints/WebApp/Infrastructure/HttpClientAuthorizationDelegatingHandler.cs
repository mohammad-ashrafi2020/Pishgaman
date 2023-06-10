using System.Net;
using Newtonsoft.Json;

namespace WebApp.Infrastructure;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrWhiteSpace(token) == false)
            {
                request.Headers.Add("Authorization", token);
            }
        }

        var res = await base.SendAsync(request, cancellationToken);
        if (res.StatusCode == HttpStatusCode.Unauthorized)
        {
            res.Content = new StringContent(JsonConvert.SerializeObject(new ApiResult()
            {
                IsSuccess = false,
                MetaData = new MetaData()
                {
                    Message = "Unauthorized",
                    AppStatusCode = AppStatusCode.UnAuthorize
                }
            }));
            res.StatusCode = HttpStatusCode.OK;
        }
        if (res.StatusCode == HttpStatusCode.InternalServerError)
        {
            res.Content.Dispose();
            res.Content = new StringContent(JsonConvert.SerializeObject(new ApiResult()
            {
                IsSuccess = false,
                MetaData = new MetaData()
                {
                    Message = "خطای سمت سرور",
                    AppStatusCode = AppStatusCode.ServerError
                }
            }));
            res.StatusCode = HttpStatusCode.OK;
        }
        return res;
    }
}
