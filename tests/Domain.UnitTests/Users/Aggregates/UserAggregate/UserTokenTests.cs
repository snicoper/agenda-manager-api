using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserAggregate;

public class UserTokenTests
{
    [Fact]
    public void UserToken_ShouldAdded_WhenUserTokenIsAddedToUser()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var userToken = UserTokenFactory.CreateUserToken();

        // Act
        user.AddUserToken(userToken);

        // Assert
        user.Tokens.Should().Contain(userToken);
    }

    [Fact]
    public void UserToken_ShouldRemoved_WhenUserTokenIsRemovedFromUser()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var userToken = UserTokenFactory.CreateUserToken();
        user.AddUserToken(userToken);

        // Act
        user.RemoveUserToken(userToken);

        // Assert
        user.Tokens.Should().NotContain(userToken);
    }

    [Fact]
    public void UserToken_ShouldRaiseEvent_WhenUserTokenIsAddedToUser()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var userToken = UserTokenFactory.CreateUserToken();

        // Act
        user.AddUserToken(userToken);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserTokenAddedDomainEvent);
    }

    [Fact]
    public void UserToken_ShouldRaiseEvent_WhenUserTokenIsRemovedFromUser()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var userToken = UserTokenFactory.CreateUserToken();
        user.AddUserToken(userToken);

        // Act
        user.RemoveUserToken(userToken);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserTokenRemovedDomainEvent);
    }
}
