using AgendaManager.Domain.Common.ValueObjects.Duration;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.DurationTests;

public class DurationTests
{
    [Fact]
    public void Duration_ShouldCreate_WhenValidDurationIsProvidedWithHoursAndMinutes()
    {
        // Arrange
        const int hours = 1;
        const int minutes = 2;

        // Act
        var duration = Duration.From(hours, minutes);

        // Assert
        duration.Value.Should().Be(TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
    }

    [Fact]
    public void Duration_ShouldCreate_WhenValidDurationIsProvidedWithDaysAndHoursAndMinutes()
    {
        // Arrange
        const int days = 1;
        const int hours = 1;
        const int minutes = 2;

        // Act
        var duration = Duration.From(days, hours, minutes);

        // Assert
        duration.Value.Should().Be(TimeSpan.FromDays(days) + TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes));
    }

    [Fact]
    public void Duration_ShouldCreate_WhenValidDurationIsProvidedWithTimeSpan()
    {
        // Arrange
        var timeSpan = TimeSpan.FromHours(1);

        // Act
        var duration = Duration.From(timeSpan);

        // Assert
        duration.Value.Should().Be(timeSpan);
    }
}
