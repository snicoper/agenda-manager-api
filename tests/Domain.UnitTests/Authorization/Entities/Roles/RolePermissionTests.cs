using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.Entities.Roles;

public class RolePermissionTests
{
    private readonly Role _role = RoleFactory.CreateRole();

    [Fact]
    public void RolePermission_ShouldRaiseEvent_WhenPermissionAdded()
    {
        // Arrange
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Act
        _role.AddPermission(permission);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is RolePermissionAddedDomainEvent);
        _role.Permissions.Should().Contain(permission);
        _role.Permissions.Should().HaveCount(1);
    }

    [Fact]
    public void RolePermission_ShouldRaiseEvent_WhenPermissionRemoved()
    {
        // Arrange
        var permission = PermissionFactory.CreatePermissionUsersCreate();
        _role.AddPermission(permission);

        // Act
        _role.RemovePermission(permission);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is RolePermissionRemovedDomainEvent);
        _role.Permissions.Should().NotContain(permission);
        _role.Permissions.Should().HaveCount(0);
    }
}
