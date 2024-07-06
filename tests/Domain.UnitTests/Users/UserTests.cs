using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories.Users;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users;

public class UserTests
{
    [Fact]
    public void User_ShouldReturnUser_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUser();

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void User_ShouldRaiseEvent_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUser();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }
}
