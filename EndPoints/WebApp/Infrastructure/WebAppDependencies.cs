using Application.Persons;
using WebApp.Services.Auth;
using WebApp.Services.Person;

namespace WebApp.Infrastructure;

public static class WebAppDependencies
{
    public static IServiceCollection AddWebAppDependencies(this IServiceCollection services)
    {
        var baseAddress = "https://localhost:5001/api/";

        services.AddHttpContextAccessor();

        services.AddScoped<HttpClientAuthorizationDelegatingHandler>();
        //services.AddTransient<IRenderViewToString, RenderViewToString>();

        //services.AddCookieManager();

        services.AddHttpClient<IAuthService, AuthService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();


        services.AddHttpClient<IHttpPersonService, HttpPersonService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();


        return services;
    }
}