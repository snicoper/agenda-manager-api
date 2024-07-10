using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ValueObjects;

public class RefreshTokenTests
{
    [Fact]
    public void RefreshToken_ShouldCreateNewRefreshToken_WhenValidRefreshTokenIsPassed()
    {
        // Act
        var refreshToken = RefreshTokenFactory.CreateValidRefreshToken();

        // Assert
        refreshToken.Token.Should().NotBeNull();
        refreshToken.ExpiryTime.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenInvalidTokenIsPassed()
    {
        // Act
        var refreshToken = RefreshTokenFactory.CreateInvalidToken;

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenExpiredRefreshTokenIsPassed()
    {
        // Act
        var refreshToken = RefreshTokenFactory.CreateExpiredRefreshTime;

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RefreshToken_ShouldNotCreateRefreshToken_WhenEmptyTokenIsPassed()
    {
        // Act
        var refreshToken = RefreshTokenFactory.CreateEmptyToken;

        // Assert
        refreshToken.Should().Throw<ArgumentException>();
    }
}
