﻿using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Entities.Roles;

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
