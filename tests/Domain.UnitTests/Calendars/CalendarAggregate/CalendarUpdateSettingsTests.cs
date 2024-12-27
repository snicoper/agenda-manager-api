using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarUpdateSettingsTests
{
    [Fact]
    public void UpdateSettings_ShouldSuccess_WhenSettingsIsUpdated()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(timeZone: IanaTimeZone.FromIana("America/New_York"));
        var calendar = CalendarFactory.CreateCalendar();
        calendar.UpdateSettings(settings);

        // Act
        var updatedSettings = CalendarSettingsFactory.CreateCalendarSettings();
        var result = calendar.UpdateSettings(updatedSettings);

        // Assert
        calendar.Settings.Should().Be(updatedSettings);
        result.Should().BeTrue();
    }

    [Fact]
    public void UpdateSettings_ShouldRaiseEvent_WhenSettingsIsUpdated()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(timeZone: IanaTimeZone.FromIana("America/New_York"));
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        var result = calendar.UpdateSettings(settings);

        // Assert
        result.Should().BeTrue();
        calendar.DomainEvents.Should().ContainSingle(e => e is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateSettings_ShouldNotRaiseEvent_WhenSettingsIsNotUpdated()
    {
        // Arrange
        var settings =
            CalendarSettingsFactory.CreateCalendarSettings(timeZone: IanaTimeZone.FromIana("America/New_York"));
        var calendar = CalendarFactory.CreateCalendar(settings: settings);
        calendar.UpdateSettings(settings);

        // Act
        var result = calendar.UpdateSettings(settings);

        // Assert
        result.Should().BeFalse();
        calendar.DomainEvents.Should().NotContain(e => e is CalendarSettingsUpdatedDomainEvent);
    }
}
