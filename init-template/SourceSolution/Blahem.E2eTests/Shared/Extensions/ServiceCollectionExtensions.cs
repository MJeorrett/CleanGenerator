using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Blahem.E2eTests.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceServiceWithMock<T>(this IServiceCollection services)
        where T : class
    {
        services.RemoveServiceOfType<T>();

        var mockService = new Mock<T>();
        services.AddSingleton(mockService);
        services.AddSingleton(mockService.Object);

        return services;
    }

    public static IServiceCollection RemoveServiceOfType<T>(this IServiceCollection services)
        where T : class
    {
        var descriptor = services.FirstOrDefault(_ => _.ServiceType == typeof(T));

        if (descriptor is null)
        {
            throw new InvalidOperationException($"No service is registered with type {typeof(T)}.");
        }

        services.Remove(descriptor);

        return services;
    }
}
