using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using FluentAssertions;

namespace AgendaManager.Domain.UnitTests.Common.ValueObjects.ColcorSchemeTests;

public class ColorSchemeTests
{
    [Fact]
    public void ColorScheme_ShouldCreateNewColorScheme_WhenValidColorSchemeIsProvided()
    {
        // Arrange
        const string text = "text";
        const string background = "background";

        // Act
        var colorScheme = ColorScheme.From(text, background);

        // Assert
        colorScheme.Text.Should().Be(text);
        colorScheme.Background.Should().Be(background);
    }
}
