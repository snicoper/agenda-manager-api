using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.UserAggregate;

public class UserRoleTests
{
    [Fact]
    public void UserRole_ShouldRaiseEvent_WhenRoleIsAdded()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var role = RoleFactory.CreateRole();

        // Act
        user.AddRole(role);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRoleAddedDomainEvent);
        user.Roles.Should().Contain(role);
        user.Roles.Should().HaveCount(1);
        user.Roles.Should().ContainSingle(x => x.Id == role.Id);
    }

    [Fact]
    public void UserRole_ShouldRaiseEvent_WhenRoleIsRemoved()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();
        var role = RoleFactory.CreateRole();
        user.AddRole(role);

        // Act
        user.RemoveRole(role);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserRoleRemovedDomainEvent);
        user.Roles.Should().NotContain(role);
        user.Roles.Should().HaveCount(0);
    }
}
