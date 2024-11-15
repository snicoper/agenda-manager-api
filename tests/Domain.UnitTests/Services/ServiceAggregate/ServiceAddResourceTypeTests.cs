using AgendaManager.Domain.Services.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Services.ServiceAggregate;

public class ServiceAddResourceTypeTests
{
    [Fact]
    public void AddResourceType_ShouldSetResourceType_WhenResourceTypeIsAdded()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        var resourceType = ResourceTypeFactory.CreateResourceType();

        // Act
        service.AddResourceType(resourceType);

        // Assert
        service.ResourceTypes.Should().Contain(resourceType);
    }

    [Fact]
    public void AddResourceType_ShouldRaiseEvent_WhenResourceTypeIsAdded()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        var resourceType = ResourceTypeFactory.CreateResourceType();

        // Act
        service.AddResourceType(resourceType);

        // Assert
        service.DomainEvents.Should().Contain(x => x is ResourceTypeAddedToServiceDomainEvent);
    }
}
