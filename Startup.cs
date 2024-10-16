using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Trains.Data;
using Trains.Shared;
using Xunit;
using Route = Trains.Data.Route;

namespace Trains;

public class Startup
{
    public void ConfigureServices(IServiceCollection services, HostBuilderContext builder)
    {
        services.AddTransient<Railway>();
    }
}