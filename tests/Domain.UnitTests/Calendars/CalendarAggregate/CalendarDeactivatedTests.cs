using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarDeactivatedTests
{
    [Fact]
    public void Activate_ShouldChangedState_WhenCalendarIsActivated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(isActive: true);

        // Act
        calendar.Deactivate();

        // Assert
        calendar.IsActive.Should().BeFalse();
    }

    [Fact]
    public void Activate_ShouldRaiseEvent_WhenCalendarIsDeactivated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(isActive: true);

        // Act
        calendar.Deactivate();

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarDeactivatedDomainEvent);
    }

    [Fact]
    public void Activate_ShouldNotRaiseEvent_WhenCalendarIsAlreadyInactive()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(isActive: false);

        // Act
        calendar.Deactivate();

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarDeactivatedDomainEvent);
    }
}
