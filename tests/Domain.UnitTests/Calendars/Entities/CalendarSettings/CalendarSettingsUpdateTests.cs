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
        var configuration = CalendarSettingsConfigurationFactory.CreateConfiguration(
            IanaTimeZone.FromIana(IanaTimeZoneConstants.AsiaTokyo),
            HolidayStrategy.AllowOverlapping,
            AppointmentStrategy.AllowOverlapping);

        // Act
        _settings.Update(configuration);

        // Assert
        _settings.IanaTimeZone.Should().Be(configuration.IanaTimeZone);
        _settings.HolidayStrategy.Should().Be(configuration.HolidayStrategy);
        _settings.AppointmentStrategy.Should().Be(configuration.AppointmentStrategy);
    }
}
