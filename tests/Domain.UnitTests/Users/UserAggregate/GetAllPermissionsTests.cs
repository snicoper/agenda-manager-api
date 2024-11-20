using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class GetAllPermissionsTests
{
    [Fact]
    public void GetAllPermissions_ShouldReturnAllPermissions()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        var role = RoleFactory.CreateRole();
        var role1 = RoleFactory.CreateRole();
        var role2 = RoleFactory.CreateRole();

        var permission = PermissionFactory.CreatePermission();
        var permission1 = PermissionFactory.CreatePermission();
        var permission2 = PermissionFactory.CreatePermission();

        user.AddRole(role);
        user.AddRole(role1);
        user.AddRole(role2);

        role.AddPermission(permission);
        role.AddPermission(permission1);
        role.AddPermission(permission2);

        // Act
        var result = user.GetAllPermissions();

        // Assert
        result.Should().HaveCount(3);
    }
}
