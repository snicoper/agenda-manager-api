using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarSettings;

public class CalendarHasChangesTests
{
    [Fact]
    public void HasChanges_ShouldReturnFalse_WhenSettingsAreTheSame()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings();

        // Act
        var hasChanges = settings.HasChanges(settings);

        // Assert
        hasChanges.Should().BeFalse();
    }

    [Fact]
    public void HasChanges_ShouldReturnTrue_WhenSettingsAreDifferent()
    {
        // Arrange
        var settings = CalendarSettingsFactory.CreateCalendarSettings();
        var newSettings =
            CalendarSettingsFactory.CreateCalendarSettings(timeZone: IanaTimeZone.FromIana("America/New_York"));

        // Act
        var hasChanges = settings.HasChanges(newSettings);

        // Assert
        hasChanges.Should().BeTrue();
    }
}
