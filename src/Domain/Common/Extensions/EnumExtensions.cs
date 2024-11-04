using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Domain.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var enumType = value.GetType();
        var enumValue = Enum.GetName(enumType, value);

        if (enumValue is null)
        {
            return value.ToString();
        }

        var memberInfo = enumType.GetMember(enumValue).FirstOrDefault();

        if (memberInfo is null)
        {
            return enumValue;
        }

        if (memberInfo
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() is not DisplayAttribute displayAttribute)
        {
            return enumValue;
        }

        return displayAttribute.ResourceType is not null
            ? displayAttribute.GetName() ?? enumValue
            : displayAttribute.Name ?? enumValue;
    }
}
