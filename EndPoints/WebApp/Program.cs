using Application;
using Infrastructure;
using Infrastructure.DataBase;
using NLog;
using NLog.Config;
using NLog.Web;
using WebApp.Infrastructure;
using WebApp.Infrastructure.JwtUtil;


var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
LogManager.Configuration.Install(new InstallationContext(Console.Out));
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var configuration = builder.Configuration;
    var services = builder.Services;



    services.AddAuthorization(option =>
    {
        option.AddPolicy("Account", b =>
        {
            b.RequireAuthenticatedUser();
        });
    });
    services.AddControllers();
    services.AddRazorPages().AddRazorRuntimeCompilation()
        .AddRazorPagesOptions(options =>
        {
            options.Conventions.AuthorizeFolder("/Persons", "Account");
        });


    services
        .AddInfrastructureDependencies(configuration)
        .AddApplicationDependencies()
        .AddWebAppDependencies()
        .AddJwtAuthentication(configuration);


    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }


    app.Use(async (context, next) =>
    {
        var token = context.Request.Cookies["token"]?.ToString();
        if (string.IsNullOrWhiteSpace(token) == false)
        {
            context.Request.Headers.Append("Authorization", $"Bearer {token}");
        }
        await next();
    });

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.Use(async (context, next) =>
    {
        await next();
        var status = context.Response.StatusCode;
        if (status == 401)
        {
            var path = context.Request.Path;
            context.Response.Redirect($"/auth/login?redirectTo={path}");
        }
    });
    app.UseAuthentication();

    app.UseAuthorization();

    app.MapRazorPages();
    app.MapDefaultControllerRoute();

    GenerateDataBase(app.Services);
    app.Run();

}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

void GenerateDataBase(IServiceProvider serviceProvider)
{
    using var serviceScope = serviceProvider.CreateScope();
    var dataBaseSeeder = serviceScope.ServiceProvider.GetRequiredService<DataBaseSeeder>();
    dataBaseSeeder.SeedDatabase(serviceScope);
}
