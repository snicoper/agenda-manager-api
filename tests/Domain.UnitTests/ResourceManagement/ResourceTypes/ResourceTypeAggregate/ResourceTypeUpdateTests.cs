using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.ResourceTypes.ResourceTypeAggregate;

public class ResourceTypeUpdateTests
{
    private const string _newName = "NewName";
    private const string _newDescription = "NewDescription";
    private readonly ResourceType _resourceType = ResourceTypeFactory.CreateResourceType();

    [Fact]
    public void Update_ShouldSuccess_WhenValidDataIsProvided()
    {
        // Act
        _resourceType.Update(_newName, _newDescription);

        // Assert
        _resourceType.Name.Should().Be(_newName);
        _resourceType.Description.Should().Be(_newDescription);
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenValidDateIsProvided()
    {
        // Act
        _resourceType.Update(_newName, _newDescription);

        // Assert
        _resourceType.DomainEvents.Should().ContainSingle(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenNameAndDescriptionAreTheSame()
    {
        // Act
        _resourceType.Update(_resourceType.Name, _resourceType.Description);

        // Assert
        _resourceType.DomainEvents.Should().NotContain(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenNameOrDescriptionAreDistinct()
    {
        // Act
        _resourceType.Update(_resourceType.Name, _newDescription);

        // Assert
        _resourceType.DomainEvents.Should().Contain(x => x is ResourceTypeUpdatedDomainEvent);
    }

    [Theory]
    [InlineData("", "new description")]
    [InlineData(" ", "new description")]
    [InlineData("new name", "")]
    [InlineData("new name", " ")]
    private void Update_ShouldRaiseException_WhenInvalidDateIsProvided(string newName, string newDescription)
    {
        // Act
        var action = () => _resourceType.Update(newName, newDescription);

        // Assert
        action.Should().Throw<ResourceTypeDomainException>();
    }
}
