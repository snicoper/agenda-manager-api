using AgendaManager.Domain.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceRemoveScheduleTests
{
    [Fact]
    public void RemoveSchedule_ShouldRemove_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule);

        // Act
        resource.RemoveSchedule(schedule);

        // Assert
        resource.Schedules.Should().BeEmpty();
    }

    [Fact]
    public void RemoveSchedule_ShouldRaiseEvent_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();
        resource.AddSchedule(schedule);

        // Act
        resource.RemoveSchedule(schedule);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleRemovedDomainEvent);
    }
}
