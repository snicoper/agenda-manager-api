using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserIsActiveTests
{
    private readonly User _user = UserFactory.CreateUser();

    [Fact]
    public void UserIsActive_ShouldBeTrue_WhenUserIsCreated()
    {
        // Assert
        _user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdateActiveState_ShouldRaiseEvent_WhenUserIsActiveStateIsUpdated()
    {
        // Act
        _user.UpdateActiveState(false);

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserActiveStateUpdatedDomainEvent);
        _user.IsActive.Should().BeFalse();
    }
}
