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
        var refreshToken = RefreshToken.From(token, expiryTime);

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
        var refreshToken = () => RefreshToken.From(token, expiryTime);

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
        var refreshToken = () => RefreshToken.From(token, expiryTime);

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
        var refreshToken = () => RefreshToken.From(token, expiryTime);

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RefreshToken_ShouldCreateRefreshToken_WhenGenerateMethodIsCalled()
    {
        // Arrange
        var lifetime = TimeSpan.FromDays(1);

        // Act
        var refreshToken = RefreshToken.Generate(lifetime);

        // Assert
        refreshToken.Should().NotBeNull();
        refreshToken.Token.Should().NotBeNull();
        refreshToken.ExpiryTime.Should().BeAfter(DateTimeOffset.UtcNow);
        refreshToken.ExpiryTime.Should().BeBefore(DateTimeOffset.UtcNow.Add(lifetime));
        refreshToken.Token.Length.Should().BeLessThan(TokenLength);
    }

    [Fact]
    public void RefreshToken_ShouldNotExpireRefreshToken_WhenRefreshTokenIsCreated()
    {
        // Arrange
        var lifetime = TimeSpan.FromDays(1);

        // Act
        var refreshToken = RefreshToken.Generate(lifetime);

        // Assert
        refreshToken.Should().NotBeNull();
        refreshToken.IsExpired().Should().BeFalse();
        refreshToken.ExpiryTime.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenLifetimeIsZeroInGenerateMethod()
    {
        // Arrange
        var lifetime = TimeSpan.Zero;

        // Act
        var refreshToken = () => RefreshToken.Generate(lifetime);

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }
}
