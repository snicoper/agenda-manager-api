using System.Net;
using System.Net.Http.Json;
using AgendaManager.Application.Authentication.Models;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.TestCommon.Constants;
using AgendaManager.WebApi.Controllers.Authentication.Contracts;
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
            UserConstants.UserAlice.Email.Value,
            UserConstants.UserAlice.RawPassword);

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
            UserConstants.UserAlice.RawPassword);

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
            UserConstants.UserAlice.RawPassword);

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
            UserConstants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyEmail()
    {
        // Arrange
        var request = new LoginRequest(string.Empty, UserConstants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnValidationError_WithEmptyPassword()
    {
        // Arrange
        var request = new LoginRequest(UserConstants.UserAlice.Email.Value, string.Empty);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidEmailCredentials()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", UserConstants.UserAlice.RawPassword);

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Login_ShouldReturnConflict_WithInvalidPasswordCredentials()
    {
        // Arrange
        var request = new LoginRequest(UserConstants.UserAlice.Email.Value, "invalid_password");

        // Act
        var response = await HttpClient.PostAsJsonAsync(Enpoints.Authentication.Login, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }
}
