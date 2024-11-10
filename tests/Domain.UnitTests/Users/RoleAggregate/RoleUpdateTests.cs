using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.RoleAggregate;

public class RoleUpdateTests
{
    [Fact]
    public void RoleUpdate_ShouldRaiseEvent_WhenUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        const string newName = "New Name";
        const string newDescription = "New Description";

        // Act
        role.UpdateRole(newName, newDescription);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public void RoleUpdate_ShouldChangeData_WhenRoleIsUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        const string newName = "New Name";
        const string newDescription = "New Description";

        // Act
        role.UpdateRole(newName, newDescription);

        // Assert
        role.Name.Should().Be(newName);
        role.Description.Should().Be(newDescription);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void RoleUpdate_ShouldThrowException_WhenNameIsInvalid(int nameLength)
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        var invalidName = new string('*', nameLength);
        const string validDescription = "Valid Description";

        // Act
        var act = () => role.UpdateRole(invalidName, validDescription);

        // Assert
        act.Should().Throw<RoleDomainException>();
    }
}
