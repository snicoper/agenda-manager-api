using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserTokenTests
{
    private readonly User _user = UserFactory.CreateUser();
    private readonly UserToken _userToken = UserTokenFactory.CreateUserToken();

    [Fact]
    public void UserToken_ShouldAdded_WhenUserTokenIsAddedToUser()
    {
        // Act
        _user.AddUserToken(_userToken);

        // Assert
        _user.Tokens.Should().Contain(_userToken);
    }

    [Fact]
    public void UserToken_ShouldRemoved_WhenUserTokenIsRemovedFromUser()
    {
        // Arrange
        _user.AddUserToken(_userToken);

        // Act
        _user.RemoveUserToken(_userToken);

        // Assert
        _user.Tokens.Should().NotContain(_userToken);
    }

    [Fact]
    public void UserToken_ShouldRaiseEvent_WhenUserTokenIsAddedToUser()
    {
        // Act
        _user.AddUserToken(_userToken);

        // Assert
        _userToken.DomainEvents.Should().Contain(x => x is UserTokenAddedDomainEvent);
    }

    [Fact]
    public void UserToken_ShouldRaiseEvent_WhenUserTokenIsRemovedFromUser()
    {
        // Arrange
        _user.AddUserToken(_userToken);

        // Act
        _user.RemoveUserToken(_userToken);

        // Assert
        _userToken.DomainEvents.Should().Contain(x => x is UserTokenRemovedDomainEvent);
    }
}
