using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserAggregate;

public class UserCreateTests
{
    [Fact]
    public void UserCreate_ShouldReturnUser_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.Should().NotBeNull();
    }

    [Fact]
    public void UserCreate_ShouldRaiseEvent_WhenUserIsCreated()
    {
        // Act
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserCreatedDomainEvent);
    }
}
