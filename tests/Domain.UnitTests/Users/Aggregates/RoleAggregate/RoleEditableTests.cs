using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.RoleAggregate;

public class RoleEditableTests
{
    [Fact]
    public void RoleEditable_ShouldRaiseEvent_WhenEditableStateUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Act
        role.UpdateEditableState(true);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleEditableStateUpdatedDomainEvent);
    }

    [Fact]
    public void RoleEditable_ShouldEditableStateBeFalse_WhenCreated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Assert
        role.Editable.Should().BeFalse();
    }

    [Fact]
    public void RoleEditable_ShouldChangeEditableState_WhenEditableStateIsSetTrue()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Act
        role.UpdateEditableState(true);

        // Assert
        role.Editable.Should().BeTrue();
    }

    [Fact]
    public void RoleEditable_ShouldChangeEditableState_WhenEditableStateIsSetFalse()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Act
        role.UpdateEditableState(false);

        // Assert
        role.Editable.Should().BeFalse();
    }
}
