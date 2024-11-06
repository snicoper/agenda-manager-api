using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Common.WekDays.Exceptions;
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

    [Fact]
    public void WekDays_ShouldThrowWeekDaysException_WhenConvertFromArrayWithMultipleSameDays()
    {
        // Arrange
        var weekDays = new[] { 1, 1, 3, 4, 5, 6, 7, 7 };

        // Act
        Action action = () => weekDays.FromNumberArray();

        // Assert
        action.Should().Throw<WeekDaysException>();
    }

    [InlineData(1, 2, 3, 4, 5, 6, 7, 8)]
    [InlineData(-2, 3, 4, 5, 6, 7, 8, 1)]
    [Theory]
    public void WekDays_ShouldThrowWeekDaysException_WhenConvertFromArrayWithOutOfRange(params int[] weekDays)
    {
        // Act
        Action action = () => weekDays.FromNumberArray();

        // Assert
        action.Should().Throw<WeekDaysException>();
    }
}
