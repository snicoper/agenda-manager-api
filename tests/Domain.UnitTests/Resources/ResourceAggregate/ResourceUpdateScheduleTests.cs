using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceUpdateScheduleTests
{
    [Fact]
    public void UpdateSchedule_ShouldSuccess_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule1 = ResourceScheduleFactory.CreateResourceSchedule();
        var schedule2 = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule1);
        resource.AddSchedule(schedule2);

        // Act
        var result = resource.UpdateSchedule(schedule1.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void UpdateSchedule_ShouldNotFound_WhenScheduleIsNotFound()
    {
        // Arrange
        var expectedError = ResourceScheduleErrors.NotFound.FirstError();
        var resource = ResourceFactory.CreateResource();
        var schedule1 = ResourceScheduleFactory.CreateResourceSchedule();
        var schedule2 = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule1);

        // Act
        var result = resource.UpdateSchedule(schedule2.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error?.FirstError().Should().Be(expectedError);
    }

    [Fact]
    public void UpdateSchedule_ShouldRaiseEvent_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule1 = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule1);

        // Act
        resource.UpdateSchedule(schedule1.Id);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleUpdatedDomainEvent);
    }
}
