using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.ResourceTypes.ResourceTypeAggregate;

public class ResourceTypeCreateTests
{
    private readonly ResourceType _resourceType = ResourceTypeFactory.CreateResourceType();

    [Fact]
    public void Create_ShouldSuccess_WhenCreateResourceTypeWithOutRoleId()
    {
        // Assert
        _resourceType.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldRaiseEvent_WhenCreateResourceType()
    {
        _resourceType.DomainEvents.Should().Contain(x => x is ResourceTypeCreatedDomainEvent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowException_WhenNameIsEmpty(string invalidName)
    {
        // Act
        var action = () => ResourceTypeFactory.CreateResourceType(name: invalidName);

        // Assert
        action.Should().Throw<ResourceTypeDomainException>()
            .WithMessage("Name is empty or exceeds length of 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Create_ShouldThrowException_WhenNameExceedsLenght(int nameLenght)
    {
        // Arrange
        var invalidName = new string('*', nameLenght);

        // Act
        var action = () => ResourceTypeFactory.CreateResourceType(name: invalidName);

        // Assert
        action.Should().Throw<ResourceTypeDomainException>()
            .WithMessage("Name is empty or exceeds length of 50 characters.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowException_WhenDescriptionIsEmpty(string invalidDescription)
    {
        // Act
        var action = () => ResourceTypeFactory.CreateResourceType(description: invalidDescription);

        // Assert
        action.Should().Throw<ResourceTypeDomainException>()
            .WithMessage("Description is invalid or exceeds length of 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Create_ShouldThrowException_WhenDescriptionExceedsLenght(int descriptionLenght)
    {
        // Arrange
        var invalidDescription = new string('*', descriptionLenght);

        // Act
        var action = () => ResourceTypeFactory.CreateResourceType(description: invalidDescription);

        // Assert
        action.Should().Throw<ResourceTypeDomainException>()
            .WithMessage("Description is invalid or exceeds length of 50 characters.");
    }
}
