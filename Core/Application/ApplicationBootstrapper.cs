using Application.Persons;
using Application.Utils.FileUtil.Services;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationBootstrapper
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
       
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<LocalFileService>();

        return services;
    }
}