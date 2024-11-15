using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserDeactivateTests
{
    [Fact]
    public void Deactivate_ShouldBeFalse_WhenUserIsCreatedWithIsActiveFalse()
    {
        // Arrange
        var user = UserFactory.CreateUser(isActive: false);

        // Assert
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_ShouldRaiseEvent_WhenIsDeactivate()
    {
        // Arrange
        var user = UserFactory.CreateUser(isActive: true);

        // Act
        user.Deactivate();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserDeactivatedDomainEvent);
        user.IsActive.Should().BeFalse();
    }
}
