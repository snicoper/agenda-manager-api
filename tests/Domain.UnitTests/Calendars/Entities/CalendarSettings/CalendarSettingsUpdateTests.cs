using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarSettingsUpdateTests
{
    private readonly Domain.Calendars.Entities.CalendarSettings _settings =
        CalendarSettingsFactory.CreateCalendarSettings();

    [Fact]
    public void Settings_ShouldSucceed_WhenValidUpdatesProvided()
    {
        // Arrange
        var newTimeZone = IanaTimeZone.FromIana(IanaTimeZoneConstants.AsiaTokyo);
        const HolidayStrategy newCreateStrategy = HolidayStrategy.AllowOverlapping;
        const AppointmentStrategy newAppointmentStrategy = AppointmentStrategy.AllowOverlapping;

        // Act
        _settings.Update(newTimeZone, newCreateStrategy, newAppointmentStrategy);

        // Assert
        _settings.IanaTimeZone.Should().Be(newTimeZone);
        _settings.HolidayStrategy.Should().Be(newCreateStrategy);
    }
}
