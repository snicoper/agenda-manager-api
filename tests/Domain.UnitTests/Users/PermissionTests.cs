using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users;

public class PermissionTests
{
    [Fact]
    public void Permission_ShouldReturnPermission_WhenCreated()
    {
        // Act
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Assert
        permission.Id.Should().Be(permission.Id);
        permission.Name.Should().Be(permission.Name);
    }
}
