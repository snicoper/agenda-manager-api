using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class HasRoleTests
{
    [Fact]
    public void HasRole_ShouldReturnTrue_WhenUserHasRole()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var role = RoleFactory.CreateRole();
        var role1 = RoleFactory.CreateRole();
        var role2 = RoleFactory.CreateRole();
        user.AddRole(role);
        user.AddRole(role1);
        user.AddRole(role2);

        // Act
        var result = user.HasRole(role.Id);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasRole_ShouldReturnFalse_WhenUserDoesNotHaveRole()
    {
        // Arrange
        var user = UserFactory.CreateUser();
        var role = RoleFactory.CreateRole();
        var role1 = RoleFactory.CreateRole();
        var role2 = RoleFactory.CreateRole();
        user.AddRole(role);
        user.AddRole(role1);
        user.AddRole(role2);

        // Act
        var result = user.HasRole(RoleId.Create());

        // Assert
        result.Should().BeFalse();
    }
}
