using Microsoft.EntityFrameworkCore;
namespace Trains.Data;

public class DatabaseService : DbContext
{
    protected readonly IConfiguration Configuration;

    public DatabaseService()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
    
    public DatabaseService(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(Configuration.GetConnectionString("RoutesDB"));
    }
    
    public DbSet<Route> Routes { get; set; }
}