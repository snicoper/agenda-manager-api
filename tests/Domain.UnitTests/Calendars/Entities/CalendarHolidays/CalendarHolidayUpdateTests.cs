using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayUpdateTests
{
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void CalendarHoliday_ShouldSucceed_WhenValidUpdatesProvided()
    {
        // Arrange
        const string newName = "New Holiday Name";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);

        // Act
        _holiday.Update(newPeriod, newName);

        // Assert
        _holiday.Period.Should().Be(newPeriod);
        _holiday.Name.Should().Be(newName);
    }

    [Fact]
    public void CalendarHolidayUpdate_ShouldRaiseDomainEvent_WhenHolidayIsUpdated()
    {
        // Arrange
        const string newName = "New Holiday Name";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);

        // Act
        _holiday.Update(newPeriod, newName);

        // Assert
        _holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayUpdatedDomainEvent);
    }
}
