using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarHolidays;

public class CalendarHolidayUpdateTests
{
    private readonly CalendarHoliday _holiday = CalendarHolidayFactory.CreateCalendarHoliday();

    [Fact]
    public void CalendarHoliday_ShouldReturnTrue_WhenValidUpdatesProvided()
    {
        // Arrange
        const string newName = "New Holiday Name";
        var newDate = new DateTime(2023, 1, 1);
        var newPeriod = Period.From(newDate, newDate);

        // Act
        var result = _holiday.Update(newPeriod, newName);

        // Assert
        _holiday.Period.Should().Be(newPeriod);
        _holiday.Name.Should().Be(newName);
        result.Should().BeTrue();
    }

    [Fact]
    public void CalendarHolidayUpdate_ShouldReturnFalse_WhenHolidayIsUpdated()
    {
        // Act
        var result = _holiday.Update(_holiday.Period, _holiday.Name);

        // Assert
        result.Should().BeFalse();
    }
}
