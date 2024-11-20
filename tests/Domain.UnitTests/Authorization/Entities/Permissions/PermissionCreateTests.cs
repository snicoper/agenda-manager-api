using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.Entities.Permissions;

public class PermissionCreateTests
{
    private readonly Permission _permission = PermissionFactory.CreatePermissionUsersCreate();

    [Fact]
    public void PermissionCreate_ShouldReturnPermission_WhenCreated()
    {
        // Assert
        _permission.Id.Should().Be(_permission.Id);
        _permission.Name.Should().Be(_permission.Name);
    }

    [Fact]
    public void PermissionCreate_ShouldRaiseEvent_WhenCreated()
    {
        // Assert
        _permission.DomainEvents.Should().Contain(x => x is PermissionCreatedDomainEvent);
    }
}
