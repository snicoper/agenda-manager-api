using AgendaManager.Domain.Calendars.Enums;
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
        const string newTimeZone = "Europe/Warsaw";
        const HolidayCreationStrategy newCreateStrategy = HolidayCreationStrategy.AllowOverlapping;

        // Act
        settings.Update(newTimeZone, newCreateStrategy);

        // Assert
        settings.TimeZone.Should().Be(newTimeZone);
        settings.HolidayCreationStrategy.Should().Be(newCreateStrategy);
    }
}
