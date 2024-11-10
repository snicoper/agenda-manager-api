using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.RoleAggregate;

public class RolePermissionTests
{
    [Fact]
    public void RolePermission_ShouldRaiseEvent_WhenPermissionAdded()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Act
        role.AddPermission(permission);

        // Assert
        role.DomainEvents.Should().Contain(x => x is RolePermissionAddedDomainEvent);
        role.Permissions.Should().Contain(permission);
        role.Permissions.Should().HaveCount(1);
    }

    [Fact]
    public void RolePermission_ShouldRaiseEvent_WhenPermissionRemoved()
    {
        // Arrange
        var role = RoleFactory.CreateRole();
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
