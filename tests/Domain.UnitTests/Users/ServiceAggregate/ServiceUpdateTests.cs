using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Common.ValueObjects.Duration;
using AgendaManager.Domain.Services.Events;
using AgendaManager.Domain.Services.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ServiceAggregate;

public class ServiceUpdateTests
{
    [Fact]
    public void Update_ShouldSetNewName_WhenServiceIsUpdated()
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Act
        var isUpdated = service.Update(
            duration: Duration.From(TimeSpan.FromMinutes(30)),
            name: "New name",
            description: "New description",
            colorScheme: ColorScheme.From("#FF0000", "#00FF00"));

        // Assert
        isUpdated.Should().BeTrue();
        service.Name.Should().Be("New name");
        service.Description.Should().Be("New description");
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenServiceIsUpdated()
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Act
        service.Update(
            duration: Duration.From(TimeSpan.FromMinutes(30)),
            name: "New name",
            description: "New description",
            colorScheme: ColorScheme.From("#FF0000", "#00FF00"));

        // Assert
        service.DomainEvents.Should().Contain(x => x is ServiceUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenServiceDataIsNotChanged()
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Act
        var isUpdated = service.Update(
            duration: service.Duration,
            name: service.Name,
            description: service.Description,
            colorScheme: service.ColorScheme);

        // Assert
        isUpdated.Should().BeFalse();
        service.DomainEvents.Should().NotContain(x => x is ServiceUpdatedDomainEvent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldThrowException_WhenServiceIsProvidedWithInvalidName(string newName)
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Act
        var action = () => service.Update(
            duration: Duration.From(TimeSpan.FromMinutes(1)),
            name: newName,
            description: "New description",
            colorScheme: ColorScheme.From("#FF0000", "#00FF00"));

        // Assert
        action.Should().Throw<ServiceDomainException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldThrowException_WhenServiceIsProvidedWithInvalidDescription(string newDescription)
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Act
        var action = () => service.Update(
            duration: Duration.From(TimeSpan.FromMinutes(1)),
            name: "New name",
            description: newDescription,
            colorScheme: ColorScheme.From("#FF0000", "#00FF00"));

        // Assert
        action.Should().Throw<ServiceDomainException>();
    }
}
