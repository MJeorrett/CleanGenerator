using Xunit;

namespace Blahem.E2eTests.Shared.WebApplicationFactory;

[CollectionDefinition(Name)]
public class CustomWebApplicationCollection : ICollectionFixture<CustomWebApplicationFixture>
{
    public const string Name = "waf";
}
