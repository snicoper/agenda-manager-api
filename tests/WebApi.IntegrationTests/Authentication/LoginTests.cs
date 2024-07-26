using System.Net;
using System.Net.Http.Json;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.WebApi.Controllers.Users.Contracts;
using FluentAssertions;

namespace AgendaManager.WebApi.UnitTests.Authentication;

public class LoginTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Login_ShouldReturnSuccess_WithValidCredentials()
    {
        // Arrange
        var request = new LoginRequest(
            TestCommon.TestConstants.Constants.UserAlice.Email.Value,
            TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResult>>();
        result?.IsSuccess.Should().BeTrue();
        result?.Value.Should().NotBeNull();
        result?.Value?.AccessToken.Should().NotBeEmpty();
        result?.Value?.RefreshToken.Should().NotBeEmpty();
        result?.Value?.Expires.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task Login_ShouldReturnConflictError_WhenUserNotEmailConfirmed()
    {
        // Arrange
        var request = new LoginRequest(
            "lexi@example.com",
            TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResult>>();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        result?.Error?.FirstError()?.Should().Be(UserErrors.EmailIsNotConfirmed);
    }

    [Fact]
    public async Task Login_ShouldReturnConflictError_WhenUserNotActive()
    {
        // Arrange
        var request = new LoginRequest(
            "lexi@example.com",
            TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<Result<TokenResult>>();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        result?.Error?.FirstError()?.Should().Be(UserErrors.UserIsNotActive);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithInvalidEmail()
    {
        // Arrange
        var request = new LoginRequest(
            "invalid_email",
            TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyEmail()
    {
        // Arrange
        var request = new LoginRequest(string.Empty, TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyPassword()
    {
        // Arrange
        var request = new LoginRequest(TestCommon.TestConstants.Constants.UserAlice.Email.Value, string.Empty);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidEmailCredentials()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", TestCommon.TestConstants.Constants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidPasswordCredentials()
    {
        // Arrange
        var request = new LoginRequest(TestCommon.TestConstants.Constants.UserAlice.Email.Value, "invalid_password");

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
