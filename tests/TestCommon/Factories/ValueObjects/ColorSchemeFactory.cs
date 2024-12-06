using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.TestCommon.Factories.ValueObjects;

public static class ColorSchemeFactory
{
    public static ColorScheme Create(string? text = null, string? background = null)
    {
        return ColorScheme.From(text ?? "#ffffff", background ?? "#000000");
    }
}
