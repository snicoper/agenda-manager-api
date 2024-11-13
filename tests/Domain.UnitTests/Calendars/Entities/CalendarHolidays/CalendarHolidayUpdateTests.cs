using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayUpdateTests
{
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void CalendarHolidayUpdate_ShouldUpdateHoliday()
    {
        // Arrange
        const string newName = "New Holiday Name";
        const string newDescription = "New Holiday Description";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);
        const WeekDays newWeekDays = WeekDays.Friday;

        // Act
        _holiday.Update(newPeriod, newWeekDays, newName, newDescription);

        // Assert
        _holiday.Period.Should().Be(newPeriod);
        _holiday.AvailableDays.Should().Be(newWeekDays);
        _holiday.Name.Should().Be(newName);
        _holiday.Description.Should().Be(newDescription);
    }

    [Fact]
    public void CalendarHolidayUpdate_ShouldRaiseDomainEvent_WhenHolidayIsUpdated()
    {
        // Arrange
        const string newName = "New Holiday Name";
        const string newDescription = "New Holiday Description";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);
        const WeekDays newWeekDays = WeekDays.Friday;

        // Act
        _holiday.Update(newPeriod, newWeekDays, newName, newDescription);

        // Assert
        _holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayUpdatedDomainEvent);
    }
}
