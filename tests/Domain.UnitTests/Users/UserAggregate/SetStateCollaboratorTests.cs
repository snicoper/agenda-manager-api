using AgendaManager.Domain.Users.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.UserAggregate;

public class SetStateCollaboratorTests
{
    [Fact]
    public void Collaborator_ShouldStateIsTrue_WhenSetStateIsTrue()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.SetStateCollaborator(true);

        // Assert
        user.Should().NotBeNull();
        user.IsCollaborator.Should().BeTrue();
    }

    [Fact]
    public void Collaborator_ShouldStateIsFalse_WhenSetStateIFalse()
    {
        // Arrange
        var user = UserFactory.CreateUser();

        // Act
        user.SetStateCollaborator(false);

        // Assert
        user.Should().NotBeNull();
        user.IsCollaborator.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Collaborator_ShouldRaiseEvent_WhenSetState(bool state)
    {
        // Arrange
        var user = UserFactory.CreateUser(isCollaborator: state);

        // Act
        user.SetStateCollaborator(!state);

        // Assert
        user.DomainEvents.Should().Contain(x => x is UserCollaboratorUpdatedDomainEvent);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Collaborator_ShouldNotRaiseEvent_WhenStateDoesNotChange(bool state)
    {
        // Arrange
        var user = UserFactory.CreateUser(isCollaborator: state);

        // Act
        user.SetStateCollaborator(state);

        // Assert
        user.DomainEvents.Should().NotContain(x => x is UserCollaboratorUpdatedDomainEvent);
    }
}
