using Api.Infrastructure.ActionFilters;
using Api.Infrastructure.JwtUtil;
using Api.Infrastructure.Middlewares;
using Api.Infrastructure.Models;
using Application;
using Infrastructure;
using Infrastructure.DataBase;
using NLog;
using NLog.Web;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var configuration = builder.Configuration;
    var services = builder.Services;
    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services
        .AddScoped<UserRequestActionFilter>()
        .AddScoped<CustomJwtValidation>()
        .AddApplicationDependencies()
        .AddInfrastructureDependencies(configuration)
        .AddJwtAuthentication(configuration);


    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseCheckRequestData();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseApiCustomExceptionHandler();

    app.MapControllers();
    GenerateDataBase(app.Services);
    InitUsers(app.Configuration);
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

void GenerateDataBase(IServiceProvider serviceProvider)
{
    using var serviceScope = serviceProvider.CreateScope();
    var dataBaseSeeder = serviceScope.ServiceProvider.GetRequiredService<DataBaseSeeder>();
    dataBaseSeeder.SeedDatabase(serviceScope);
}

void InitUsers(IConfiguration configuration)
{
    if (UsersDb.Users.Any())
        return;

    var users = configuration.GetSection("Users").Get<List<UserDto>>();
    if (users != null && users.Any())
        UsersDb.Users = users;
}