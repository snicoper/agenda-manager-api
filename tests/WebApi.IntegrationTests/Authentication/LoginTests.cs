using System.Net;
using System.Net.Http.Json;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Controllers.Authentication.Contracts;
using FluentAssertions;

namespace AgendaManager.WebApi.UnitTests.Authentication;

public class LoginTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private readonly string _uri = $"{TestCommon.TestConstants.Constants.Shared.BaseUri}/authentication/login";

    [Fact]
    public async Task Login_ShouldReturnSuccess_WithValidCredentials()
    {
        // Arrange
        var request = new LoginRequest(
            TestCommon.TestConstants.Constants.Users.Email.Value,
            TestCommon.TestConstants.Constants.Users.Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResult>>();
        result?.IsSuccess.Should().BeTrue();
        result?.Value.Should().NotBeNull();
        result?.Value?.AccessToken.Should().NotBeEmpty();
        result?.Value?.RefreshToken.Should().NotBeEmpty();
        result?.Value?.Expires.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithInvalidEmail()
    {
        // Arrange
        var request = new LoginRequest(
            "invalid_email",
            TestCommon.TestConstants.Constants.Users.Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyEmail()
    {
        // Arrange
        var request = new LoginRequest(string.Empty, TestCommon.TestConstants.Constants.Users.Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyPassword()
    {
        // Arrange
        var request = new LoginRequest(TestCommon.TestConstants.Constants.Users.Email.Value, string.Empty);

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidEmailCredentials()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", TestCommon.TestConstants.Constants.Users.Password);

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidPasswordCredentials()
    {
        // Arrange
        var request = new LoginRequest(TestCommon.TestConstants.Constants.Users.Email.Value, "invalid_password");

        // Act
        var response = await HttpClient.PostAsJsonAsync(_uri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
