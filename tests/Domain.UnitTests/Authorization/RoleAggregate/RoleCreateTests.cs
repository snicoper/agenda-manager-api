using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Events;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.RoleAggregate;

public class RoleCreateTests
{
    private readonly Role _role = RoleFactory.CreateRole();

    [Fact]
    public void RoleCreate_ShouldReturnRole_WhenCreated()
    {
        // Assert
        _role.Id.Should().Be(_role.Id);
        _role.Name.Should().Be(_role.Name);
    }

    [Fact]
    public void RoleCreate_ShouldRaiseEvent_WhenCrated()
    {
        // Assert
        _role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }
}
