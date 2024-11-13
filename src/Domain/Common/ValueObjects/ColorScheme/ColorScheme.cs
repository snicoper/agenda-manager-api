namespace AgendaManager.Domain.Common.ValueObjects.ColorScheme;

public sealed record ColorScheme
{
    private ColorScheme(string textColor, string backgroundColor)
    {
        ArgumentNullException.ThrowIfNull(textColor);
        ArgumentNullException.ThrowIfNull(backgroundColor);

        TextColor = textColor;
        BackgroundColor = backgroundColor;
    }

    public string TextColor { get; }

    public string BackgroundColor { get; }

    public static ColorScheme From(string text, string background)
    {
        return new ColorScheme(text, background);
    }
}
