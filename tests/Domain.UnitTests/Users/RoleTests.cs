using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users;

public class RoleTests
{
    [Fact]
    public void Role_ShouldReturnRole_WhenCreated()
    {
        // Act
        var role = RoleFactory.CreateRoleAdmin();

        // Assert
        role.Id.Should().Be(role.Id);
        role.Name.Should().Be(role.Name);
    }

    [Fact]
    public void Role_ShouldRaiseEvent_WhenCrated()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public void Role_ShouldRaiseEvent_WhenUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        const string newName = "New Name";
        const string newDescription = "New Description";

        // Act
        role.UpdateRole(newName, newDescription);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public void Role_ShouldRaiseEvent_WhenPermissionAdded()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Act
        role.AddPermission(permission);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RolePermissionAddedDomainEvent);
        role.Permissions.Should().Contain(permission);
        role.Permissions.Should().HaveCount(1);
    }

    [Fact]
    public void Role_ShouldRaiseEvent_WhenPermissionRemoved()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        role.AddPermission(permission);

        // Act
        role.RemovePermission(permission);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RolePermissionRemovedDomainEvent);
        role.Permissions.Should().NotContain(permission);
        role.Permissions.Should().HaveCount(0);
    }

    [Fact]
    public void Role_ShouldRaiseEvent_WhenEditableStateUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        role.UpdateEditableState(true);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleEditableStateUpdatedDomainEvent);
    }

    [Fact]
    public void Role_ShouldChangeData_WhenRoleIsUpdated()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
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
    public void Role_ShouldThrowException_WhenNameIsInvalid(int nameLength)
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        var invalidName = new string('*', nameLength);
        const string validDescription = "Valid Description";

        // Act
        var act = () => role.UpdateRole(invalidName, validDescription);

        // Assert
        act.Should().Throw<RoleDomainException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Role_ShouldThrowException_WhenDescriptionIsInvalid(int descriptionLength)
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();
        const string validName = "Valid Name";
        var invalidDescription = new string('*', descriptionLength);

        // Act
        var act = () => role.UpdateRole(validName, invalidDescription);

        // Assert
        act.Should().Throw<RoleDomainException>();
    }

    [Fact]
    public void Role_ShouldEditableStateBeFalse_WhenCreated()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Assert
        role.Editable.Should().BeFalse();
    }

    [Fact]
    public void Role_ShouldChangeEditableState_WhenEditableStateIsSetTrue()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        role.UpdateEditableState(true);

        // Assert
        role.Editable.Should().BeTrue();
    }

    [Fact]
    public void Role_ShouldChangeEditableState_WhenEditableStateIsSetFalse()
    {
        // Arrange
        var role = RoleFactory.CreateRoleAdmin();

        // Act
        role.UpdateEditableState(false);

        // Assert
        role.Editable.Should().BeFalse();
    }
}
