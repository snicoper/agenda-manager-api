using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserActivateTests
{
    [Fact]
    public void Activate_ShouldBeTrue_WhenUserIsCreatedWithIsActiveTrue()
    {
        // Arrange
        var user = UserFactory.CreateUser(isActive: true);

        // Assert
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_ShouldRaiseEvent_WhenIsActive()
    {
        // Arrange
        var user = UserFactory.CreateUser(isActive: false);

        // Act
        user.Activate();

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserActivatedDomainEvent);
        user.IsActive.Should().BeTrue();
    }
}
