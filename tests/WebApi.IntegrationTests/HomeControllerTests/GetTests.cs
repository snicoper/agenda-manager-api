using System.Net;
using System.Net.Http.Json;
using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;

namespace AgendaManager.WebApi.UnitTests.HomeControllerTests;

public class GetTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Get_ShouldReturn401Unauthorized_WhenNotAuthenticated()
    {
        // Act
        var response = await HttpClient.GetAsync("api/v1/home");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenValid()
    {
        // Arrange
        var httpClient = await GetHttpClientWithLoginAsync();
        var expected = Result.Create("Hello world");

        // Act
        var response = await httpClient.GetAsync("api/v1/home");

        // Assert
        var resultResponse = await response.Content.ReadFromJsonAsync<Result<string>>();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        resultResponse?.Value.Should().Be(expected.Value);
    }
}
