using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.TestCommon.Factories.ValueObjects;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.ColcorSchemeTests;

public class ColorSchemeTests
{
    [Theory]
    [InlineData("#FFFFFF", "#000000")]
    [InlineData("#FF0000", "#00FF00")]
    [InlineData("#0000FF", "#FFFF00")]
    public void ColorScheme_ShouldCreateNewColorScheme_WhenValidColorSchemeIsProvided(string text, string background)
    {
        // Act
        var colorScheme = ColorSchemeFactory.Create(text: text, background: background);

        // Assert
        colorScheme.Text.Should().Be(text);
        colorScheme.Background.Should().Be(background);
    }

    [Theory]
    [InlineData("FFFFFF", "#000000")]
    [InlineData("#FFF", "#000000")]
    [InlineData("#FF0000", "000000")]
    [InlineData("#FF0000", "#00FF")]
    [InlineData("#FF00G0", "#000000")]
    [InlineData("#FF0000", "#GGGGGG")]
    [InlineData("", "#000000")]
    [InlineData("#FF0000", "")]
    public void ColorScheme_ShouldRaiseException_WhenInvalidColorIsProvided(string text, string background)
    {
        // Act
        Action act = () => ColorSchemeFactory.Create(text: text, background: background);

        // Assert
        act.Should().Throw<DomainException>();
    }
}
