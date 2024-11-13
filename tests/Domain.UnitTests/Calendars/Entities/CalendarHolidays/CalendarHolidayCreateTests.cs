using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayCreateTests
{
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void CalendarHolidayCreate_ShouldReturnHoliday()
    {
        // Assert
        _holiday.Should().NotBeNull();
    }

    [Fact]
    public void CalendarHolidayCreate_ShouldRaiseEvent_WhenHolidayIsCreated()
    {
        // Assert
        _holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayCreatedDomainEvent);
    }
}
