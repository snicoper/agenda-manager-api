using AgendaManager.TestCommon.Factories;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Calendars.Entities.CalendarConfigurations;

public class CalendarConfigurationIsUnitValueTests
{
    [Fact]
    public void IsUnitValue_ShouldReturnTrue_WhenUnitValueProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfigurationUnitValue();

        var result = configuration.IsUnitValueConfiguration();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsUnitValue_ShouldReturnFalse_WhenNotUnitValueProvided()
    {
        // Arrange
        var configuration = CalendarConfigurationFactory.CreateCalendarConfiguration();

        // Act
        var result = configuration.IsUnitValueConfiguration();

        // Assert
        result.Should().BeFalse();
    }
}
