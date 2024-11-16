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
            HolidayCreateStrategy.AllowOverlapping,
            AppointmentOverlappingStrategy.AllowOverlapping);

        // Act
        _settings.Update(configuration);

        // Assert
        _settings.IanaTimeZone.Should().Be(configuration.IanaTimeZone);
        _settings.HolidayCreateStrategy.Should().Be(configuration.HolidayCreateStrategy);
        _settings.AppointmentOverlappingStrategy.Should().Be(configuration.AppointmentOverlappingStrategy);
    }
}
