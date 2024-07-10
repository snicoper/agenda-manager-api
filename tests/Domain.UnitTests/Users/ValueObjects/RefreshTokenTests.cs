using AgendaManager.Domain.Users.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class RefreshTokenTests
{
    private const int TokenLength = 200;

    [Fact]
    public void RefreshToken_ShouldCreateNewRefreshToken_WhenValidRefreshTokenIsValid()
    {
        // Arrange
        var token = Guid.NewGuid().ToString();
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var refreshToken = RefreshToken.Create(token, expiryTime);

        // Assert
        refreshToken.Token.Should().NotBeNull();
        refreshToken.ExpiryTime.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenInvalidTokenIsInvalid()
    {
        // Arrange
        var token = new string('a', TokenLength + 1);
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var refreshToken = () => RefreshToken.Create(token, expiryTime);

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenExpiredRefreshTokenIsExpired()
    {
        // Arrange
        var token = Guid.NewGuid().ToString();
        var expiryTime = DateTimeOffset.MinValue;

        // Act
        var refreshToken = () => RefreshToken.Create(token, expiryTime);

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenTokenIsEmpty()
    {
        // Arrange
        var token = string.Empty;
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var refreshToken = () => RefreshToken.Create(token, expiryTime);

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }
}
