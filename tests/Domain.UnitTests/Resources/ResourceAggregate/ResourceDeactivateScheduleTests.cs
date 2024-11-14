using AgendaManager.Domain.Resources.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Resources.ResourceAggregate;

public class ResourceDeactivateScheduleTests
{
    [Fact]
    public void ActivateSchedule_ShouldSuccess_WhenScheduleIsDeactivate()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule(isActive: true);
        resource.AddSchedule(schedule);

        // Act
        resource.DeactivateSchedule(schedule.Id);

        // Assert
        resource.Schedules[0].IsActive.Should().BeFalse();
    }

    [Fact]
    public void ActivateSchedule_ShouldRaiseEvent_WhenScheduleIsDeactivated()
    {
        // Arrange
        var resource = ResourceFactory.CreateResource();
        var schedule = ResourceScheduleFactory.CreateResourceSchedule(isActive: true);
        resource.AddSchedule(schedule);

        // Act
        resource.DeactivateSchedule(schedule.Id);

        // Assert
        resource.DomainEvents.Should().Contain(x => x is ResourceScheduleDeactivatedDomainEvent);
    }
}
