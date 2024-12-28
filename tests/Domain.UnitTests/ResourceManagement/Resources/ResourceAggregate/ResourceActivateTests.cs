using AgendaManager.Domain.ResourceManagement.Resources.Events;
using AgendaManager.Domain.ResourceManagement.Resources.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.ResourceAggregate;

public class ResourceActivateTests
{
    [Fact]
    public void Activate_ShouldActivate_WhenResourceIsNotActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: false);

        // Act
        resource.Activate();

        // Assert
        resource.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_ShouldDeactivateSetNull_WhenResourceAreActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: true);
        resource.Deactivate("Reason");

        // Act
        resource.Activate();

        // Assert
        resource.IsActive.Should().BeTrue();
        resource.DeactivationReason.Should().BeNull();
    }

    [Fact]
    public void Activate_ShouldRaiseEvent_WhenResourceIsActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: false);

        // Act
        resource.Activate();

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceActivatedDomainEvent);
    }

    [Fact]
    public void Activate_ShouldNotRaiseEvent_WhenResourceIsAlreadyActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: true);

        // Act
        resource.Activate();

        // Assert
        resource.DomainEvents.Should().NotContain(x => x is ResourceActivatedDomainEvent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Activate_ShouldThrowException_WhenInvalidNameProvided(string invalidName)
    {
        // Act
        var action = () => ResourceFactory.CreateResource(name: invalidName);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource name must be between 1 and 50 characters long.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Activate_ShouldThrowException_WhenInvalidNameLengthAreProvided(int nameLength)
    {
        // Arrange
        var invalidName = new string('*', nameLength);

        // Act
        var action = () => ResourceFactory.CreateResource(name: invalidName);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource name must be between 1 and 50 characters long.");
    }
}
