using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarAddHolidayTests
{
    [Fact]
    public void AddHoliday_ShouldAddHoliday_WhenHolidayIsAdded()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Act
        calendar.AddHoliday(holiday);

        // Assert
        calendar.Holidays.Should().Contain(holiday);
    }

    [Fact]
    public void AddHoliday_ShouldRaiseEvent_WhenHolidayIsAdded()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Act
        calendar.AddHoliday(holiday);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarHolidayAddedDomainEvent);
    }
}
