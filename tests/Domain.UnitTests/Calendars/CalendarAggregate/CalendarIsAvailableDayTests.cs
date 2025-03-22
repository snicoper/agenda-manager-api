using AgendaManager.Domain.Common.WeekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarIsAvailableDayTests
{
    [Fact]
    public void IsAvailableDay_ShouldReturnsTrue_WhenDayIsAvailable()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const DayOfWeek day = DayOfWeek.Monday;

        // Act
        var result = calendar.IsAvailableDay(day);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAvailableDay_ReturnsFalse_WhenDayIsNotAvailable()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar(availableDays: WeekDays.WorkDays);
        const DayOfWeek day = DayOfWeek.Saturday;

        // Act
        var result = calendar.IsAvailableDay(day);

        // Assert
        result.Should().BeFalse();
    }
}
