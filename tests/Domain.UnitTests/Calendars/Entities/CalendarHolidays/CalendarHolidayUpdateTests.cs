using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayUpdateTests
{
    [Fact]
    public void CalendarHolidayUpdate_ShouldUpdateHoliday()
    {
        // Arrange
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();
        const string newName = "New Holiday Name";
        const string newDescription = "New Holiday Description";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);
        const WeekDays newWeekDays = WeekDays.Friday;

        // Act
        holiday.Update(newPeriod, newWeekDays, newName, newDescription);

        // Assert
        holiday.Period.Should().Be(newPeriod);
        holiday.AvailableDays.Should().Be(newWeekDays);
        holiday.Name.Should().Be(newName);
        holiday.Description.Should().Be(newDescription);
    }

    [Fact]
    public void CalendarHolidayUpdate_ShouldRaiseDomainEvent_WhenHolidayIsUpdated()
    {
        // Arrange
        var holiday = CalendarHolidayFactory.CreateCalendarHoliday();
        const string newName = "New Holiday Name";
        const string newDescription = "New Holiday Description";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);
        const WeekDays newWeekDays = WeekDays.Friday;

        // Act
        holiday.Update(newPeriod, newWeekDays, newName, newDescription);

        // Assert
        holiday.DomainEvents.Should().Contain(x => x is CalendarHolidayUpdatedDomainEvent);
    }
}
