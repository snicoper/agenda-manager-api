using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserRoleTests
{
    private readonly User _user = UserFactory.CreateUser();
    private readonly Role _role = RoleFactory.CreateRole();

    [Fact]
    public void UserRole_ShouldRaiseEvent_WhenRoleIsAdded()
    {
        // Arrange
        var userRole = UserRoleFactory.CreateUserRole(_user.Id, _role.Id);

        // Act
        _user.AddRole(userRole);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is UserRoleAddedDomainEvent);
        _user.UserRoles.Should().Contain(userRole);
        _user.UserRoles.Should().HaveCount(1);
        _user.UserRoles.Should().ContainSingle(x => x.RoleId == _role.Id);
    }

    [Fact]
    public void UserRole_ShouldRaiseEvent_WhenRoleIsRemoved()
    {
        // Arrange
        var userRole = UserRoleFactory.CreateUserRole(_user.Id, _role.Id);
        _user.AddRole(userRole);

        // Act
        _user.RemoveRole(userRole);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is UserRoleRemovedDomainEvent);
        _user.UserRoles.Should().NotContain(userRole);
        _user.UserRoles.Should().HaveCount(0);
    }
}
