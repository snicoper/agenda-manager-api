using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ServiceAggregate;

public class ServiceRemoveResourceType
{
    [Fact]
    public void RemoRemoveResourceTypeveResource_ShouldRemoveResourceType_WhenResourceTypeIsRemoved()
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
    public void RemoveResource_ShouldRemoveResourceType_WhenResourceTypeIsNotRemoved()
    {
        // Arrange
        var service = ServiceFactory.CreateService();
        var resourceType = ResourceTypeFactory.CreateResourceType();

        // Act
        service.RemoveResourceType(resourceType);

        // Assert
        service.ResourceTypes.Should().NotContain(resourceType);
    }
}
