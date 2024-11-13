using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarAddHolidayTests
{
    private readonly Calendar _calendar = CalendarFactory.CreateCalendar();
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void AddHoliday_ShouldAddHoliday_WhenHolidayIsAdded()
    {
        // Act
        _calendar.AddHoliday(_holiday);

        // Assert
        _calendar.Holidays.Should().Contain(_holiday);
    }

    [Fact]
    public void AddHoliday_ShouldRaiseEvent_WhenHolidayIsAdded()
    {
        // Act
        _calendar.AddHoliday(_holiday);

        // Assert
        _calendar.DomainEvents.Should().Contain(x => x is CalendarHolidayAddedDomainEvent);
    }
}
