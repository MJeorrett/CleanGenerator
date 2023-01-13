using Blahem.Application.Blaitems.Commands.Create;
using Blahem.Application.Common.AppRequests;
using Microsoft.Extensions.DependencyInjection;

namespace Blahem.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddRequestHandlers(services);

        return services;
    }

    private static void AddRequestHandlers(IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<CreateBlaitemCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsSelf()
                .WithScopedLifetime();

            scan.FromAssemblyOf<CreateBlaitemCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsSelf()
                .WithScopedLifetime();
        });
    }
}
