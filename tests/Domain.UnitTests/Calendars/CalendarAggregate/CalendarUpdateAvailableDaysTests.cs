using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarUpdateAvailableDaysTests
{
    [Fact]
    public void UpdateAvailableDays_ShouldUpdateAvailableDays_WithValidDates()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const WeekDays availableDays = WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday;

        // Act
        calendar.UpdateAvailableDays(availableDays);

        // Assert
        calendar.AvailableDays.Should().Be(availableDays);
    }

    [Fact]
    public void UpdateAvailableDays_ShouldRaiseEvent_WhenUpdate()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const WeekDays availableDays = WeekDays.Monday | WeekDays.Tuesday | WeekDays.Wednesday;

        // Act
        calendar.UpdateAvailableDays(availableDays);

        // Assert
        calendar.DomainEvents.Should().Contain(x => x is CalendarAvailableDaysUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateAvailableDays_ShouldNotUpdateAvailableDays_WithSameAvailableDays()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const WeekDays availableDays = WeekDays.All;

        // Act
        calendar.UpdateAvailableDays(availableDays);

        // Assert
        calendar.AvailableDays.Should().Be(availableDays);
    }

    [Fact]
    public void UpdateAvailableDays_ShouldNotRaiseEvent_WhenAvailableDaysAreSame()
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();
        const WeekDays availableDays = WeekDays.All;

        // Act
        calendar.UpdateAvailableDays(availableDays);

        // Assert
        calendar.DomainEvents.Should().NotContain(x => x is CalendarAvailableDaysUpdatedDomainEvent);
    }
}
