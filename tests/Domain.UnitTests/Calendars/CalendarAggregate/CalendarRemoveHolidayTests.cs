using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarRemoveHolidayTests
{
    [Fact]
    public void RemoveHoliday_ShouldRemoveHoliday_WhenHolidayIsRemoved()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Act
        calendar.AddHoliday(holiday);
        calendar.RemoveHoliday(holiday);

        // Assert
        calendar.Holidays.Should().NotContain(holiday);
    }

    [Fact]
    public void RemoveHoliday_ShouldRaiseEvent_WhenHolidayIsRemoved()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Act
        calendar.RemoveHoliday(holiday);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarHolidayRemovedDomainEvent);
    }
}
