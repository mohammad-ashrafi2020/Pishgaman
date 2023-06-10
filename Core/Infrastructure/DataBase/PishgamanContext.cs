using Infrastructure.Entities.Persons;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase;

class PishgamanContext : DbContext
{
    public PishgamanContext(DbContextOptions<PishgamanContext> options) : base(options)
    {

    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<NlogDBLog> NlogDBLog { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PishgamanContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}