using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserTokens;

public class UserTokenConsume
{
    [Fact]
    public void UserToken_ShouldReturnSuccess_WhenTokenInvalid()
    {
        // Arrange
        var userToken = UserTokenFactory.CreateUserToken();
        var token = userToken.Token.Value;

        // Act
        var result = userToken.Consume(token);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void UserToken_ShouldReturnFailureInvalidToken_WhenTokenIsInvalid()
    {
        // Arrange
        var userToken = UserTokenFactory.CreateUserToken();
        const string token = "Invalid token";

        // Act
        var result = userToken.Consume(token);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError()?.Description.Should().Be("User token is invalid.");
    }

    [Fact]
    public void UserToken_ShouldRaiseEvent_WhenUserTokenIsConsumed()
    {
        // Arrange
        var userToken = UserTokenFactory.CreateUserToken();
        var token = userToken.Token.Value;

        // Act
        userToken.Consume(token);

        // Assert
        userToken.DomainEvents.Should().Contain(x => x is UserTokenConsumedDomainEvent);
    }
}
