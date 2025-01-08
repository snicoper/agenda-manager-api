using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record ColorScheme
{
    private ColorScheme(string text, string background)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(background);

        ValidHexColor(text);
        ValidHexColor(background);

        Text = text;
        Background = background;
    }

    public string Text { get; }

    public string Background { get; }

    public static ColorScheme From(string text, string background)
    {
        return new ColorScheme(text, background);
    }

    private static void ValidHexColor(string hexColor)
    {
        if (!DomainRegex.ValidHexColor().IsMatch(hexColor))
        {
            throw new DomainException(
                $"Invalid hex color {hexColor}. Must start with # and contain exactly 6 hexadecimal characters.");
        }
    }
}
