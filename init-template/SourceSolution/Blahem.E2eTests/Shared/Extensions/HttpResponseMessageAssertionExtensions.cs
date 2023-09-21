using Blahem.Application.Common.AppRequests;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blahem.E2eTests.Shared.Extensions;

public static class HttpResponseMessageAssertionsExtensions
{
    public static async Task<AndConstraint<HttpResponseMessageAssertions>> HaveStatusCode(
        this HttpResponseMessageAssertions target,
        int expectedStatusCode)
    {
        var subject = target.Subject;

        var responseContent = expectedStatusCode != (int)subject.StatusCode ?
            await subject.Content.ReadAsStringAsync() :
            "";

        Execute.Assertion
            .ForCondition((int)subject.StatusCode == expectedStatusCode)
            .FailWith($"Expected status code {expectedStatusCode} but received status code {(int)subject.StatusCode} with " +
                (string.IsNullOrEmpty(responseContent) ?
                    "no content." :
                    $"content:\n{responseContent.Replace("{", "{{").Replace("}", "}}")}"));

        return new AndConstraint<HttpResponseMessageAssertions>(target);
    }

    public static async Task<AndConstraint<HttpResponseMessageAssertions>> HaveSuccessStatusCode(
        this HttpResponseMessageAssertions target)
    {
        var subject = target.Subject;

        var responseContent = subject.IsSuccessStatusCode ?
            "" :
            await subject.Content.ReadAsStringAsync();

        Execute.Assertion
            .ForCondition(subject.IsSuccessStatusCode)
            .FailWith($"Expected status code to be success but received status code {(int)subject.StatusCode} with " +
                (string.IsNullOrEmpty(responseContent) ?
                    "no content." :
                    $"content:\n{responseContent.Replace("{", "{{").Replace("}", "}}")}"));

        return new AndConstraint<HttpResponseMessageAssertions>(target);
    }


    public static async Task<AndConstraint<HttpResponseMessageAssertions>> HaveMessage(
        this HttpResponseMessageAssertions target,
        string expectedMessage)
    {
        var subject = target.Subject;

        var appResponse = await subject.Content.ReadFromJsonAsync<AppResponse>();

        appResponse.Should().NotBeNull();

        Execute.Assertion
            .ForCondition(appResponse!.Message == expectedMessage)
            .FailWith($"Expected message \"{expectedMessage}\" but received message \"{appResponse.Message}\"");

        return new AndConstraint<HttpResponseMessageAssertions>(target);
    }

    private record ErrorResponse
    {
        public record Error
        {
            public string Name { get; init; } = "";
        }

        public List<Error> Errors { get; init; } = new();
    }

    public static async Task<AndConstraint<HttpResponseMessageAssertions>> HaveStatusCode400WithErrorForField(this HttpResponseMessageAssertions target, string expectedErrorField)
    {
        var subject = target.Subject;

        var responseContent = await subject.Content.ReadAsStringAsync();

        Execute.Assertion
            .ForCondition((int)subject.StatusCode == 400)
            .FailWith($"Expected status code 400 but received status code {(int)subject.StatusCode} with " +
                (string.IsNullOrEmpty(responseContent) ?
                    "no content." :
                    $"content:\n{responseContent.Replace("{", "{{").Replace("}", "}}")}"))
            .Then
            .Given(() =>
            {
                try
                {
                    var errorResponse = JsonDocument.Parse(responseContent);

                    return Tuple.Create(true, (JsonDocument?)errorResponse);
                }
                catch (Exception)
                {
                    return Tuple.Create<bool, JsonDocument?>(false, null);
                }
            })
            .ForCondition(deserializationResult => deserializationResult.Item1)
            .FailWith("Expected 400 response content to be standard format: {{ errors: [ {{ name: string }} ]}}.")
            .Then
            .ForCondition(deserializationResult => deserializationResult.Item2!.RootElement.TryGetProperty("errors", out var errors))
            .FailWith("Expected response content to have key 'errors'.")
            .Then
            .Given(deserializationResult => deserializationResult.Item2!.RootElement.GetProperty("errors"))
            .ForCondition(errors => errors.TryGetProperty(expectedErrorField, out _))
            .FailWith("Expected error for: {0}\nFound error key(s): {1}.",
                _ => expectedErrorField,
                errors => string.Join(",", errors.EnumerateObject().Select(_ => _.Name)));

        return new AndConstraint<HttpResponseMessageAssertions>(target);
    }
}
