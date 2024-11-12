using AgendaManager.Domain.Calendars.Enums;
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
}
