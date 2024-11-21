using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.UserRoles;

public class UserRoleTests
{
    [Fact]
    public void UserRole_ShouldCreateUserRole()
    {
        // Act
        var userRole = UserRoleFactory.CreateUserRole();

        // Assert
        userRole.Should().NotBeNull();
        userRole.UserId.Should().Be(userRole.UserId);
        userRole.RoleId.Should().Be(userRole.RoleId);
    }
}
