using System.Globalization;
using System.Text;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Domain.Common.Extensions;

public static class StringExtensions
{
    public static string ToLowerFirstLetter(this string value)
    {
        var result = string.IsNullOrWhiteSpace(value) ? value : $"{value[..1].ToLower()}{value[1..]}";

        return result;
    }

    public static string ToUpperFirstLetter(this string value)
    {
        var result = string.IsNullOrWhiteSpace(value) ? value : $"{value[..1].ToUpper()}{value[1..]}";

        return result;
    }

    public static string ToTitle(this string value)
    {
        var result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLower());

        return result;
    }

    public static string? NullIfEmpty(this string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    public static string EmptyIfNull(this string? value)
    {
        return value ?? string.Empty;
    }

    public static string Slugify(this string text)
    {
        const string replacement = "-";

        var result = DomainRegex.Slugify()
            .Replace(RemoveDiacritics(text), replacement)
            .Replace("--", "-")
            .Trim('-');

        return result;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.ToLower().Trim().Normalize(NormalizationForm.FormD);

        StringBuilder stringBuilder = new();

        foreach (var character in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(character);
            }
        }

        var result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        return result;
    }
}
