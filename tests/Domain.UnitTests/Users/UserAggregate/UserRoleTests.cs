using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
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
        // Act
        _user.AddRole(_role);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is UserRoleAddedDomainEvent);
        _user.Roles.Should().Contain(_role);
        _user.Roles.Should().HaveCount(1);
        _user.Roles.Should().ContainSingle(x => x.Id == _role.Id);
    }

    [Fact]
    public void UserRole_ShouldRaiseEvent_WhenRoleIsRemoved()
    {
        // Arrange
        _user.AddRole(_role);

        // Act
        _user.RemoveRole(_role);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is UserRoleRemovedDomainEvent);
        _user.Roles.Should().NotContain(_role);
        _user.Roles.Should().HaveCount(0);
    }
}
