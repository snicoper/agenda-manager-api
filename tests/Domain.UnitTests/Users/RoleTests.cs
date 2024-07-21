using AgendaManager.Domain.Users.Events;
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
    public void Role_ShouldRaiseEvent_WhenAddingPermission()
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
    public void Role_ShouldRaiseEvent_WhenRemovingPermission()
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
}
