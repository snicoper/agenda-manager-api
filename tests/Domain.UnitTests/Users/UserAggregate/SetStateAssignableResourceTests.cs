using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class SetStateAssignableResourceTests
{
    [Fact]
    public void AssignableResource_ShouldStateIsTrue_WhenSetStateIsTrue()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.SetStateAssignableResource(true);

        // Assert
        user.Should().NotBeNull();
        user.IsAssignableResource.Should().BeTrue();
    }

    [Fact]
    public void AssignableResource_ShouldStateIsFalse_WhenSetStateIFalse()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.SetStateAssignableResource(false);

        // Assert
        user.Should().NotBeNull();
        user.IsAssignableResource.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AssignableResource_ShouldRaiseEvent_WhenSetState(bool state)
    {
        // Arrange
        var user = UserFactory.CreateUser(isAssignableResource: state);

        // Act
        user.SetStateAssignableResource(!state);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserAssignableResourceUpdatedDomainEvent);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void AssignableResource_ShouldNotRaiseEvent_WhenStateDoesNotChange(bool state)
    {
        // Arrange
        var user = UserFactory.CreateUser(isAssignableResource: state);

        // Act
        user.SetStateAssignableResource(state);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserAssignableResourceUpdatedDomainEvent);
    }
}
