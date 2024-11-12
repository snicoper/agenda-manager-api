using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarSettingsUpdateTests
{
    [Fact]
    public void Update_ShouldUpdateCalendarSettings()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings();
        var newTimeZone = IanaTimeZone.FromIana("Europe/Warsaw");
        const HolidayCreationStrategy newCreateStrategy = HolidayCreationStrategy.AllowOverlapping;

        // Act
        settings.Update(newTimeZone, newCreateStrategy);

        // Assert
        settings.IanaTimeZone.Should().Be(newTimeZone);
        settings.HolidayCreationStrategy.Should().Be(newCreateStrategy);
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenUpdateSettingsWithValidTimeZone()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings();
        var newTimeZone = IanaTimeZone.FromIana("Europe/Warsaw");
        const HolidayCreationStrategy newCreateStrategy = HolidayCreationStrategy.AllowOverlapping;

        // Act
        settings.Update(newTimeZone, newCreateStrategy);

        // Assert
        settings.DomainEvents.Should().ContainSingle(e => e is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenUpdateSettingsWithSameValues()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings();

        // Act
        settings.Update(settings.IanaTimeZone, settings.HolidayCreationStrategy);

        // Assert
        settings.DomainEvents.Should().NotContain(x => x is CalendarSettingsUpdatedDomainEvent);
    }
}
