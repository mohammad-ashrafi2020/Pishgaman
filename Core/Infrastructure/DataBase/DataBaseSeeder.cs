using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataBase;

public class DataBaseSeeder
{

    public void SeedDatabase(IServiceScope serviceScope)
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<PishgamanContext>();
        context.Database.EnsureCreated();
    }
}