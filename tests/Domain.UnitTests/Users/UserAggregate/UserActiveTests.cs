using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserActiveTests
{
    private readonly User _user = UserFactory.CreateUser();

    [Fact]
    public void UserActivate_ShouldActiveTrue_WhenUserIsACreated()
    {
        // Assert
        _user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UserActivate_ShouldRaiseEvent_WhenUserIsActiveStateIsUpdated()
    {
        // Act
        _user.UpdateActiveState(false);

        // Assert
        _user.DomainEvents.Should().Contain(x => x is UserActiveStateUpdatedDomainEvent);
        _user.IsActive.Should().BeFalse();
    }
}
