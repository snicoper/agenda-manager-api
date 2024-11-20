using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Events;
using AgendaManager.Domain.Authorization.Exceptions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Authorization.Entities.Roles;

public class RoleUpdateTests
{
    private readonly Role _role = RoleFactory.CreateRole();

    [Fact]
    public void RoleUpdate_ShouldRaiseEvent_WhenUpdated()
    {
        // Arrange
        const string newName = "New Name";
        const string newDescription = "New Description";

        // Act
        _role.Update(newName, newDescription);

        // Assert
        _role.DomainEvents.Should().Contain(x => x is RoleCreatedDomainEvent);
    }

    [Fact]
    public void RoleUpdate_ShouldChangeData_WhenRoleIsUpdated()
    {
        // Arrange
        const string newName = "New Name";
        const string newDescription = "New Description";

        // Act
        _role.Update(newName, newDescription);

        // Assert
        _role.Name.Should().Be(newName);
        _role.Description.Should().Be(newDescription);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void RoleUpdate_ShouldThrowException_WhenNameIsInvalid(int nameLength)
    {
        // Arrange
        var invalidName = new string('*', nameLength);
        const string validDescription = "Valid Description";

        // Act
        var action = () => _role.Update(invalidName, validDescription);

        // Assert
        action.Should().Throw<RoleDomainException>();
    }
}
