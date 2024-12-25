using AgendaManager.Domain.Calendars.Events;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarSettingsCreateTests
{
    [Fact]
    public void Create_ShouldReturnCalendarSettings_WhenIsCreated()
    {
        // Act
        var settings = CalendarSettingsFactory.CreateCalendarSettings();

        // Assert
        settings.Should().NotBeNull();
        settings.CalendarId.Should().Be(settings.CalendarId);
    }

    [Fact]
    public void Create_ShouldRaiseEvent_WhenIsCreated()
    {
        // Act
        var settings = CalendarSettingsFactory.CreateCalendarSettings();

        // Assert
        settings.DomainEvents.Should().NotBeEmpty();
        settings.DomainEvents.Should().Contain(x => x is CalendarSettingsCreatedDomainEvent);
    }
}
