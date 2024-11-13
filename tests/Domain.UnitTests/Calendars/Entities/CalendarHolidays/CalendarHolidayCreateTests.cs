using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayCreateTests
{
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void CalendarHoliday_ShouldCreate_WhenValidParametersProvided()
    {
        // Assert
        _holiday.Should().NotBeNull();
    }

    [Fact]
    public void CalendarHoliday_ShouldRaiseEvent_WhenValidParametersProvided()
    {
        // Assert
        _holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayCreatedDomainEvent);
    }
}
