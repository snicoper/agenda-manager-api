using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Common.WekDays.Extensions;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.WebDays;

public class WeekDaysTests
{
    [Fact]
    public void WeekDays_ShouldBeEnum()
    {
        // Act
        var result = typeof(WeekDays).IsEnum;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void WekDays_ShouldReturnArray_WhenConvertWeekDaysToArray()
    {
        // Act
        var result = WeekDays.All.ToNumberArray();

        // Assert
        result.Length.Should().Be(7);
    }

    [Fact]
    public void WekDays_ShouldReturnAll_WhenConvertFromArray()
    {
        // Arrange
        var weekDays = new[] { 1, 2, 3, 4, 5, 6, 7 };

        // Act
        var result = weekDays.FromNumberArray();

        // Assert
        result.Should().Be(WeekDays.All);
    }

    [Fact]
    public void WekDays_ShouldReturnNone_WhenConvertFromArray()
    {
        // Arrange
        var weekDays = Array.Empty<int>();

        // Act
        var result = weekDays.FromNumberArray();

        // Assert
        result.Should().Be(WeekDays.None);
    }
}
