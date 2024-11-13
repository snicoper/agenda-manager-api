using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserCreateTests
{
    private readonly User _user = UserFactory.CreateUser();

    [Fact]
    public void UserCreate_ShouldReturnUser_WhenUserIsCreated()
    {
        // Assert
        _user.Should().NotBeNull();
    }

    [Fact]
    public void UserCreate_ShouldRaiseEvent_WhenUserIsCreated()
    {
        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }
}
