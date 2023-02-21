using Blahem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blahem.E2eTests.Shared.WebApplicationFactory;

public class CustomWebApplicationFixture
{
    public CustomWebApplicationFactory Factory = null!;


    public CustomWebApplicationFixture()
    {
        Factory = new CustomWebApplicationFactory();

        var services = Factory.Services.CreateScope().ServiceProvider;
        EnsureDatabasesCreatedAndMigrated(services);
    }

    private static void EnsureDatabasesCreatedAndMigrated(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<CustomWebApplicationFixture>>();

        try
        {
            logger.LogInformation("Migrating database.");

            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            logger.LogInformation("Database migration done.");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception trying to ensure database created and migrated.");
            throw;
        }
    }
}
