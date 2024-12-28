using AgendaManager.Domain.ResourceManagement.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.ResourceManagement.Resources.ResourceAggregate;

public class ResourceScheduleAddScheduleTests
{
    [Fact]
    public void AddSchedule_ShouldAdd_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();

        // Act
        resource.AddSchedule(schedule);

        // Assert
        resource.Schedules.Should().Contain(schedule);
    }

    [Fact]
    public void AddSchedule_ShouldRaiseEvent_WhenScheduleIsProvided()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule();

        // Act
        resource.AddSchedule(schedule);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleCreatedDomainEvent);
    }
}
