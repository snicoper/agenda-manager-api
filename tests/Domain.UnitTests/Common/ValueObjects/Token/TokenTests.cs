using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.Token;

public class TokenTests
{
    private const int TokenLength = 200;

    [Fact]
    public void Token_ShouldCreateNewRefreshToken_WhenValidRefreshTokenIsValid()
    {
        // Arrange
        var token = Guid.NewGuid().ToString();
        var expires = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var refreshToken = Domain.Common.ValueObjects.Token.Token.From(token, expires);

        // Assert
        refreshToken.Value.Should().NotBeNull();
        refreshToken.Expires.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Token_ShouldNotCreateRefreshToken_WhenInvalidTokenIsInvalid()
    {
        // Arrange
        var token = new string('a', TokenLength + 1);
        var expires = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var action = () => Domain.Common.ValueObjects.Token.Token.From(token, expires);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Token_ShouldNotCreateRefreshToken_WhenExpiredRefreshTokenIsExpired()
    {
        // Arrange
        var token = Guid.NewGuid().ToString();
        var expires = DateTimeOffset.MinValue;

        // Act
        var action = () => Domain.Common.ValueObjects.Token.Token.From(token, expires);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Token_ShouldNotCreateRefreshToken_WhenTokenIsEmpty()
    {
        // Arrange
        var token = string.Empty;
        var expires = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var action = () => Domain.Common.ValueObjects.Token.Token.From(token, expires);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Token_ShouldCreateRefreshToken_WhenGenerateMethodIsCalled()
    {
        // Arrange
        var lifetime = TimeSpan.FromDays(1);

        // Act
        var refreshToken = Domain.Common.ValueObjects.Token.Token.Generate(lifetime);

        // Assert
        refreshToken.Should().NotBeNull();
        refreshToken.Value.Should().NotBeNull();
        refreshToken.Expires.Should().BeAfter(DateTimeOffset.UtcNow);
        refreshToken.Expires.Should().BeBefore(DateTimeOffset.UtcNow.Add(lifetime));
        refreshToken.Value.Length.Should().BeLessThan(TokenLength);
    }

    [Fact]
    public void Token_ShouldNotExpireRefreshToken_WhenRefreshTokenIsCreated()
    {
        // Arrange
        var lifetime = TimeSpan.FromDays(1);

        // Act
        var refreshToken = Domain.Common.ValueObjects.Token.Token.Generate(lifetime);

        // Assert
        refreshToken.Should().NotBeNull();
        refreshToken.IsExpired.Should().BeFalse();
        refreshToken.Expires.Should().BeAfter(DateTimeOffset.UtcNow);
    }

    [Fact]
    public void Token_ShouldNotCreateRefreshToken_WhenLifetimeIsZeroInGenerateMethod()
    {
        // Arrange
        var lifetime = TimeSpan.Zero;

        // Act
        var action = () => Domain.Common.ValueObjects.Token.Token.Generate(lifetime);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Token_ShouldReturnTrue_WhenIsExpiredIsCalled()
    {
        // Arrange
        var token = Domain.Common.ValueObjects.Token.Token.Generate(TimeSpan.FromDays(1));

        // Act
        var result = token.IsExpired;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Token_ShouldReturnTrue_WhenValidateTokenWithTokenIsValid()
    {
        // Arrange
        var token = Domain.Common.ValueObjects.Token.Token.Generate(TimeSpan.FromDays(1));

        // Act
        var result = token.Validate(token.Value);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Token_ShouldReturnFalse_WhenValidateTokenWithTokenIsNotValid()
    {
        // Arrange
        var token = Domain.Common.ValueObjects.Token.Token.Generate(TimeSpan.FromDays(1));
        const string invalidToken = "invalidToken";

        // Act
        var result = token.Validate(invalidToken);

        // Assert
        result.Should().BeFalse();
    }
}
