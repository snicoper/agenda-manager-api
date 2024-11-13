using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserCreateTests
{
    private readonly User _user = UserFactory.CreateUser();

    [Fact]
    public void User_ShouldNotBeNull_WhenUserIsCreated()
    {
        // Assert
        _user.Should().NotBeNull();
    }

    [Fact]
    public void CreatingUser_ShouldRaiseUserCreatedEvent_WhenUserIsCreated()
    {
        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }
}
