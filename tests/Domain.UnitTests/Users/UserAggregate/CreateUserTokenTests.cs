using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class CreateUserTokenTests
{
    [Fact]
    public void CreateUserToken_ShouldCreateUserToken()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.EmailConfirmation);

        // Assert
        result.Should().NotBeNull();
        user.Tokens.Should().Contain(x => x == result.Value);
    }

    [Fact]
    public void CreateUserToken_ShouldRaiseEvent_WhenUserTokenIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.EmailConfirmation);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value?.DomainEvents.Should().Contain(x => x is UserTokenCreatedDomainEvent);
    }

    [Fact]
    public void CreateUserToken_ShouldLifetime_WhenEmailConfirmationTokenIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.EmailConfirmation);

        // Assert
        result.Should().NotBeNull();
        user.Tokens.Should().Contain(x => x == result.Value);
        result.Value?.Token.Expires.Should().BeBefore(DateTimeOffset.UtcNow.AddMinutes(11));
        result.Value?.Token.Expires.Should().BeAfter(DateTimeOffset.UtcNow.AddMinutes(9));
    }

    [Fact]
    public void CreateUserToken_ShouldLifetime_WhenPasswordResetTokenIsCreated()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        var result = user.CreateUserToken(UserTokenType.PasswordReset);

        // Assert
        result.Should().NotBeNull();
        user.Tokens.Should().Contain(x => x == result.Value);
        result.Value?.Token.Expires.Should().BeBefore(DateTimeOffset.UtcNow.AddDays(8));
        result.Value?.Token.Expires.Should().BeAfter(DateTimeOffset.UtcNow.AddDays(6));
    }
}
