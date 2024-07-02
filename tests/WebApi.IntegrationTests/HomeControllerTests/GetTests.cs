using System.Net;
using System.Net.Http.Json;
using AgendaManager.Domain.Common.Responses;
using FluentAssertions;

namespace AgendaManager.WebApi.UnitTests.HomeControllerTests;

public class GetTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly string _uri = ComposeUrl("home");

    [Fact]
    public async Task Get_ShouldReturn401Unauthorized_WhenUserIsNotAuthenticated()
    {
        // Act
        var response = await HttpClient.GetAsync(_uri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenUserIsAuthenticated()
    {
        // Arrange
        var httpClient = await GetHttpClientWithLoginAsync();
        var expected = Result.Create("Hello world");

        // Act
        var response = await httpClient.GetAsync(_uri);
        var resultResponse = await response.Content.ReadFromJsonAsync<Result<string>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultResponse?.Value.Should().Be(expected.Value);
    }
}
