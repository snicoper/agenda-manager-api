﻿using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class UserActiveTests
{
    [Fact]
    public void UserActivate_ShouldActiveTrue_WhenUserIsACreated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Assert
        user.Active.Should().BeTrue();
    }

    [Fact]
    public void UserActivate_ShouldRaiseEvent_WhenUserIsActiveStateIsUpdated()
    {
        // Arrange
        var user = UserFactory.CreateUserAlice();

        // Act
        user.UpdateActiveState(false);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserActiveStateUpdatetedDomainEvent);
        user.Active.Should().BeFalse();
    }
}
