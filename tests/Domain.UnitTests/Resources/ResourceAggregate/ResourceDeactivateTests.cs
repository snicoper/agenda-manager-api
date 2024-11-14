using AgendaManager.Domain.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceDeactivateTests
{
    [Fact]
    public void Deactivate_ShouldDeactivate_WhenResourceIsNotActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: true);

        // Act
        resource.Deactivate("Reason");

        // Assert
        resource.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_ShouldRaiseEvent_WhenResourceAreDeactivated()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: true);

        // Act
        resource.Deactivate("Reason");

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceDeactivatedDomainEvent);
    }

    [Fact]
    public void Deactivate_ShouldNotDeactivate_WhenResourceIsNotActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource(isActive: false);

        // Act
        resource.Deactivate("Reason");

        // Assert
        resource.DomainEvents.Should().NotContain(x => x is ResourceDeactivatedDomainEvent);
    }
}
