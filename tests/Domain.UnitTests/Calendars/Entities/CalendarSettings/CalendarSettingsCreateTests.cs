using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarSettingsCreateTests
{
    [Fact]
    public void Settings_ShouldCreate_WhenValidParametersProvided()
    {
        // Arrange && Act
        var calendarId = CalendarId.Create();
        var settings = CalendarSettingsFactory.CreateCalendarSettings(calendarId: calendarId);

        settings.Should().NotBeNull();
        settings.CalendarId.Should().Be(calendarId);
    }

    [Fact]
    public void Settings_ShouldRaiseEvent_WhenValidParametersProvided()
    {
        // Arrange && Act
        var settings = CalendarSettingsFactory.CreateCalendarSettings();

        // Assert
        settings.DomainEvents.Should().Contain(x => x is CalendarSettingsCreatedDomainEvent);
    }
}
