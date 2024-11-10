using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.Aggregates.PermissionAggregate;

public class PermissionCreateTests
{
    [Fact]
    public void PermissionCreate_ShouldReturnPermission_WhenCreated()
    {
        // Act
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Assert
        permission.Id.Should().Be(permission.Id);
        permission.Name.Should().Be(permission.Name);
    }

    [Fact]
    public void PermissionCreate_ShouldRaiseEvent_WhenCreated()
    {
        // Arrange
        var permission = PermissionFactory.CreatePermissionUsersCreate();

        // Assert
        permission.DomainEvents.Should().Contain(x => x is PermissionCreatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void PermissionCreate_ShouldRaiseException_WhenInvalidNameIsSet(int nameLength)
    {
        // Arrange
        var name = new string('*', nameLength);

        // Assert
        Assert.Throws<PermissionDomainException>(() => PermissionFactory.CreatePermission(name: name));
    }
}
