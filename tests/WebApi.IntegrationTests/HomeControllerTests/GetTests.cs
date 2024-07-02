using System.Net;
using AgendaManager.Domain.Common.Abstractions;
using FluentAssertions;

namespace AgendaManager.WebApi.UnitTests.HomeControllerTests;

public class GetTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact(Skip = "Implementar dependencias de repositorio")]
    public async Task Get_ShouldReturnOk_WhenValid()
    {
        // Arrange
        await LoginAsync();
        var expected = Result.Create("Hello world");

        // Act
        var result = await HttpClient.GetAsync("api/home");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
