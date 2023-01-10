using Blahem.Application.Common.Interfaces;
using Blahem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blahem.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlahemDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                builder => builder.MigrationsAssembly(typeof(BlahemDbContext).Assembly.FullName)));

        services.AddScoped<IBlahemDbContext>(provider => provider.GetRequiredService<BlahemDbContext>());
    }
}
