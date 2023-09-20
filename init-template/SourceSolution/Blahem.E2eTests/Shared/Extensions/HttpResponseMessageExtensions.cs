using Blahem.Application.Common.AppRequests;
using System.Net.Http.Json;

namespace Blahem.E2eTests.Shared.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> ReadOptionalResponseContentAs<T>(this HttpResponseMessage message)
    {
        try
        {
            var response = await message.Content.ReadFromJsonAsync<AppResponse<T>>();

            return response == null ? default : response.Content;
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to parse response content as {typeof(T).Name}.", exception);
        }
    }

    public static async Task<T> ReadResponseContentAs<T>(this HttpResponseMessage message)
    {
        var responseContent = await message.ReadOptionalResponseContentAs<T>();

        if (responseContent is null) throw new Exception($"Failed to parse response content as {typeof(T).Name}.");

        return responseContent;
    }
}
