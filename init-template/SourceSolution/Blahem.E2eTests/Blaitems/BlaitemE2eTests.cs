using Blahem.E2eTests.Shared.Dtos.Blaitems;
using Blahem.E2eTests.Shared.Endpoints;
using Blahem.E2eTests.Shared.Extensions;
using Blahem.E2eTests.Shared.WebApplicationFactory;
using FluentAssertions;
using Xunit.Abstractions;

namespace Blahem.E2eTests.Blaitems;

[Collection(CustomWebApplicationCollection.Name)]
public class BlaitemE2eTests : TestBase
{
    public BlaitemE2eTests(
        CustomWebApplicationFixture webApplicationFixture,
        ITestOutputHelper testOutputHelper) :
        base(webApplicationFixture, testOutputHelper)
    {
    }

    [Fact]
    public async Task ShouldReturnCreatedBlaitemById()
    {
        var createResponse = await HttpClient.CreateBlaitem().Call(new() { Title = "Expected title" });

        await createResponse.Should().HaveStatusCode(201);

        var createdBlaitemId = await createResponse.ReadResponseContentAs<int>();

        var getBlaitemByIdResponse = await HttpClient.GetBlaitemById().Call(createdBlaitemId);

        await getBlaitemByIdResponse.Should().HaveStatusCode(200);

        var returnedBlaitem = await getBlaitemByIdResponse.ReadResponseContentAs<BlaitemDto>();

        returnedBlaitem.Title.Should().Be("Expected title");
    }

    [Fact]
    public async Task ShouldListCreatedBlaitems()
    {
        var blaitem1Id = await HttpClient.CreateBlaitem().CallAndParseResponse(new() { Title = "Title 1" });
        var blaitem2Id = await HttpClient.CreateBlaitem().CallAndParseResponse(new() { Title = "Title 2" });

        var listBlaitemsResponse = await HttpClient.ListBlaitems().Call();

        await listBlaitemsResponse.Should().HaveStatusCode(200);

        var listBlaitemsResults = await listBlaitemsResponse.ReadResponseContentAs<List<BlaitemDto>>();

        listBlaitemsResults.Should().HaveCount(2);
        listBlaitemsResults[0].Should().BeEquivalentTo(new BlaitemDto() { Id = blaitem1Id, Title = "Title 1" });
        listBlaitemsResults[1].Id.Should().Be(blaitem2Id);
    }

    [Fact]
    public async Task ShouldUpdateBlaitem()
    {
        var blaitemId = await HttpClient.CreateBlaitem().CallAndParseResponse(new() { Title = "Title 1" });

        var updateResponse = await HttpClient.UpdateBlaitem().Call(new() { Id = blaitemId, Title = "Updated title" });

        await updateResponse.Should().HaveStatusCode(200);

        var updatedBlaitem = await HttpClient.GetBlaitemById().CallAndParseResponse(blaitemId);

        updatedBlaitem.Should().BeEquivalentTo(new BlaitemDto() { Id = blaitemId, Title = "Updated title" });
    }

    [Fact]
    public async Task ShouldDeleteBlaitem()
    {
        var blaitemId1 = await HttpClient.CreateBlaitem().CallAndParseResponse(new() { Title = "Title 1" });
        var blaitemId2 = await HttpClient.CreateBlaitem().CallAndParseResponse(new() { Title = "Title 2" });

        var deleteBlaitemResponse = await HttpClient.DeleteBlaitem().Call(blaitemId1);

        await deleteBlaitemResponse.Should().HaveStatusCode(200);

        var listBlaitemsResponse = await HttpClient.ListBlaitems().CallAndParseResponse();

        listBlaitemsResponse.Should().HaveCount(1);
        listBlaitemsResponse[0].Id.Should().Be(blaitemId2);
    }
}
