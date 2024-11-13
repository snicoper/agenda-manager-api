using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarRemoveHolidayTests
{
    private readonly Calendar _calendar = CalendarFactory.CreateCalendar();
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void RemoveHoliday_ShouldSucceed_WhenValidHolidayProvided()
    {
        // Act
        _calendar.AddHoliday(_holiday);
        _calendar.RemoveHoliday(_holiday);

        // Assert
        _calendar.Holidays.Should().NotContain(_holiday);
    }

    [Fact]
    public void RemoveHoliday_ShouldRaiseEvent_WhenValidHolidayProvided()
    {
        // Act
        _calendar.RemoveHoliday(_holiday);

        // Assert
        _calendar.DomainEvents.Should().Contain(x => x is CalendarHolidayRemovedDomainEvent);
    }
}
