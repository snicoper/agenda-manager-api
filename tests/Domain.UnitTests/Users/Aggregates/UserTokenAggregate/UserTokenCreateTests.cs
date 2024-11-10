using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserTokenAggregate;

public class UserTokenCreateTests
{
    [Fact]
    public void UserTokenCreate_ShouldReturnUserToken_WhenUserTokenIsCreated()
    {
        // Act
        var userToken = UserTokenFactory.CreateUserToken();

        // Assert
        userToken.Should().NotBeNull();
    }

    [Fact]
    public void UserTokenCreate_ShouldRaiseEvent_WhenUserTokenIsCreated()
    {
        // Act
        var userToken = UserTokenFactory.CreateUserToken();

        // Assert
        userToken.DomainEvents.Should().Contain(x => x is UserTokenCreatedDomainEvent);
    }
}
