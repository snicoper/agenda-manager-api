using AgendaManager.Domain.Resources.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceCreateTests
{
    [Fact]
    public void Create_ShouldReturnResource_ValidResource()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();

        // Assert
        resource.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowException_WhenInvalidNameProvided(string invalidName)
    {
        var action = () => ResourceFactory.CreateResource(name: invalidName);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource name must be between 1 and 50 characters long.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(257)]
    public void Create_ShouldThrowException_WhenInvalidNameLengthAreProvided(int nameLength)
    {
        // Arrange
        var invalidName = new string('*', nameLength);

        // Act
        var action = () => ResourceFactory.CreateResource(name: invalidName);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource name must be between 1 and 50 characters long.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowException_WhenInvalidDescriptionProvided(string invalidDescription)
    {
        // Act
        var action = () => ResourceFactory.CreateResource(description: invalidDescription);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource description must be between 1 and 500 characters long.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Create_ShouldThrowException_WhenInvalidDescriptionLengthAreProvided(int descriptionLength)
    {
        // Arrange
        var invalidDescription = new string('*', descriptionLength);

        // Act
        var action = () => ResourceFactory.CreateResource(description: invalidDescription);

        // Assert
        action.Should().Throw<ResourceDomainException>()
            .WithMessage("Resource description must be between 1 and 500 characters long.");
    }
}
