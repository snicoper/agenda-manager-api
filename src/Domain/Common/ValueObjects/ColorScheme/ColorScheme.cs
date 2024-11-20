namespace AgendaManager.Domain.Common.ValueObjects.ColorScheme;

public sealed record ColorScheme
{
    private ColorScheme(string text, string background)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(background);

        Text = text;
        Background = background;
    }

    public string Text { get; }

    public string Background { get; }

    public static ColorScheme From(string text, string background)
    {
        return new ColorScheme(text, background);
    }
}
