using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarSettingsUpdateTests
{
    private readonly Domain.Calendars.Entities.CalendarSettings _settings =
        CalendarSettingsFactory.CreateCalendarSettings();

    [Fact]
    public void Update_ShouldUpdateCalendarSettings()
    {
        // Arrange
        var newTimeZone = IanaTimeZone.FromIana("Europe/Warsaw");
        const HolidayCreationStrategy newCreateStrategy = HolidayCreationStrategy.AllowOverlapping;

        // Act
        _settings.Update(newTimeZone, newCreateStrategy);

        // Assert
        _settings.IanaTimeZone.Should().Be(newTimeZone);
        _settings.HolidayCreationStrategy.Should().Be(newCreateStrategy);
    }

    [Fact]
    public void Update_ShouldRaiseEvent_WhenUpdateSettingsWithValidTimeZone()
    {
        // Arrange
        var newTimeZone = IanaTimeZone.FromIana("Europe/Warsaw");
        const HolidayCreationStrategy newCreateStrategy = HolidayCreationStrategy.AllowOverlapping;

        // Act
        _settings.Update(newTimeZone, newCreateStrategy);

        // Assert
        _settings.DomainEvents.Should().ContainSingle(e => e is CalendarSettingsUpdatedDomainEvent);
    }

    [Fact]
    public void Update_ShouldNotRaiseEvent_WhenUpdateSettingsWithSameValues()
    {
        // Act
        _settings.Update(_settings.IanaTimeZone, _settings.HolidayCreationStrategy);

        // Assert
        _settings.DomainEvents.Should().NotContain(x => x is CalendarSettingsUpdatedDomainEvent);
    }
}
