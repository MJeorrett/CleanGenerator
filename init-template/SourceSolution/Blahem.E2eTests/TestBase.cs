using Blahem.Application.Common.Interfaces;
using Blahem.E2eTests.Shared.WebApplicationFactory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

namespace Blahem.E2eTests;

public class TestBase : IAsyncLifetime
{
    protected readonly CustomWebApplicationFactory Factory;
    protected readonly CustomWebApplicationFixture Fixture;

    protected IServiceProvider Services { get; init; }

    protected HttpClient HttpClient;

    public IApplicationDbContext ResolveDbContext() => Services.GetRequiredService<IApplicationDbContext>();

    public TestBase(CustomWebApplicationFixture webApplicationFixture, ITestOutputHelper testOutputHelper)
    {
        Fixture = webApplicationFixture;
        Factory = webApplicationFixture.Factory;
        Services = Factory.GetScopedServiceProvider();
        HttpClient = Factory.CreateClient();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.TestOutput(testOutputHelper)
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .CreateLogger();
    }

    public virtual async Task InitializeAsync()
    {
        await Factory.ResetState();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
