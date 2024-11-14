using AgendaManager.Domain.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceActiveScheduleTests
{
    [Fact]
    public void ActivateSchedule_ShouldSuccess_WhenScheduleIsActive()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule(isActive: false);
        resource.AddSchedule(schedule);

        // Act
        resource.ActiveSchedule(schedule.Id);

        // Assert
        resource.Schedules[0].IsActive.Should().BeTrue();
    }

    [Fact]
    public void ActivateSchedule_ShouldRaiseEvent_WhenScheduleIsActivated()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule(isActive: false);
        resource.AddSchedule(schedule);

        // Act
        resource.ActiveSchedule(schedule.Id);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleActivatedDomainEvent);
    }
}
