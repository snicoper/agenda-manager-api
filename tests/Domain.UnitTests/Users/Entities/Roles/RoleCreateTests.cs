using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.Roles;

public class RoleCreateTests
{
    [Fact]
    public void RoleCreate_ShouldReturnRole_WhenCreated()
    {
        // Act
        var role = RoleFactory.CreateRole();

        // Assert
        role.Id.Should().Be(role.Id);
        role.Name.Should().Be(role.Name);
    }

    [Fact]
    public void RoleCreate_ShouldRaiseEvent_WhenCrated()
    {
        // Arrange
        var role = RoleFactory.CreateRole();

        // Assert
        role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }
}
