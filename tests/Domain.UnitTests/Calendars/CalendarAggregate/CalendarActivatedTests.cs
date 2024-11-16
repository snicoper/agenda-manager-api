using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarActivatedTests
{
    [Fact]
    public void Activate_ShouldChangedState_WhenCalendarIsDeactivated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(isActive: false);

        // Act
        calendar.Activate();

        // Assert
        calendar.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Activate_ShouldRaiseEvent_WhenCalendarIsActivated()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(isActive: false);

        // Act
        calendar.Activate();

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarActivatedDomainEvent);
    }

    [Fact]
    public void Activate_ShouldNotRaiseEvent_WhenCalendarIsAlreadyActive()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.Activate();

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarActivatedDomainEvent);
    }
}
