using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class HasPermissionTests
{
    [Fact]
    public void HasPermission_ShouldReturnTrue_WhenUserHasPermission()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var role = RoleFactory.CreateRole();

        user.AddRole(role);

        var permission = PermissionFactory.CreatePermission();
        var permission1 = PermissionFactory.CreatePermission();
        var permission2 = PermissionFactory.CreatePermission();

        role.AddPermission(permission);
        role.AddPermission(permission1);
        role.AddPermission(permission2);

        // Act
        var result = user.HasPermission(permission.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasPermission_ShouldReturnFalse_WhenUserDoesNotHavePermission()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var role = RoleFactory.CreateRole();

        user.AddRole(role);

        var permission = PermissionFactory.CreatePermission();
        var permission1 = PermissionFactory.CreatePermission();
        var permission2 = PermissionFactory.CreatePermission();

        role.AddPermission(permission);
        role.AddPermission(permission1);
        role.AddPermission(permission2);

        // Act
        var result = user.HasPermission(PermissionId.Create());

        // Assert
        result.Should().BeFalse();
    }
}
