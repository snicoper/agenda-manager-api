using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.CalendarAggregate;

public class CalendarSettingsUpdateSettingsTests
{
    [Theory]
    [InlineData(IanaTimeZoneConstants.AmericaNewYork)]
    [InlineData(IanaTimeZoneConstants.EuropeMadrid)]
    [InlineData(IanaTimeZoneConstants.Utc)]
    public void Settings_ShouldUpdateSuccessfully_WhenValidParametersProvided(string newTimeZone)
    {
        // Arrange
        var calendar = CalendarFactory.CreateCalendar();

        // Act
        calendar.UpdateSettings(IanaTimeZone.FromIana(newTimeZone), calendar.Settings.HolidayCreationStrategy);

        // Assert
        calendar.Settings.IanaTimeZone.Value.Should().Be(newTimeZone);
    }
}
