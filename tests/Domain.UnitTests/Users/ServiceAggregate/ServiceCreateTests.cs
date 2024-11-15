using AgendaManager.Domain.Services.Events;
using AgendaManager.Domain.Services.Exceptions;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Users.ServiceAggregate;

public class ServiceCreateTests
{
    [Fact]
    public void Create_ShouldCreated_WhenServiceIsProvided()
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Assert
        service.Should().NotBeNull();
    }

    [Fact]
    public void Create_ShouldRaiseEvent_WhenServiceIsCreated()
    {
        // Arrange
        var service = ServiceFactory.CreateService();

        // Assert
        service.DomainEvents.Should().Contain(x => x is ServiceCreatedDomainEvent);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(51)]
    public void Create_ShouldRaiseException_WhenServiceIsProvidedWithInvalidName(int nameLength)
    {
        // Arrange
        var name = new string('*', nameLength);

        // Act
        var action = () => ServiceFactory.CreateService(name: name);

        // Assert
        action.Should().Throw<ServiceDomainException>()
            .WithMessage("Service name must be between 1 and 50 characters.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(501)]
    public void Create_ShouldRaiseException_WhenServiceIsProvidedWithInvalidDescription(int descriptionLength)
    {
        // Arrange
        var description = new string('*', descriptionLength);

        // Act
        var action = () => ServiceFactory.CreateService(description: description);

        // Assert
        action.Should().Throw<ServiceDomainException>()
            .WithMessage("Service description must be between 1 and 500 characters.");
    }
}
