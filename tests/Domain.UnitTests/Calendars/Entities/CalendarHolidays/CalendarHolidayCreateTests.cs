using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayCreateTests
{
    [Fact]
    public void CalendarHolidayCreate_ShouldReturnHoliday()
    {
        // Arrange
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Assert
        holiday.Should().NotBeNull();
    }

    [Fact]
    public void CalendarHolidayCreate_ShouldRaiseEvent_WhenHolidayIsCreated()
    {
        // Arrange
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();

        // Assert
        holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayCreatedDomainEvent);
    }
}
