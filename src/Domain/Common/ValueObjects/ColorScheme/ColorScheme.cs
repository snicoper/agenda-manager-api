using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Common.ValueObjects.ColorScheme;

public class ColorScheme : ValueObject
{
    private ColorScheme(string textColor, string backgroundColor)
    {
        TextColor = textColor;
        BackgroundColor = backgroundColor;
    }

    public string TextColor { get; }

    public string BackgroundColor { get; }

    public static ColorScheme From(string text, string background)
    {
        return new ColorScheme(text, background);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TextColor;
        yield return BackgroundColor;
    }
}
