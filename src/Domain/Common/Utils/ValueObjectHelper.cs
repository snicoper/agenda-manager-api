using System.Reflection;

namespace AgendaManager.Domain.Common.Utils;

/// <summary>
/// Utility class for working with value objects.
/// </summary>
public static class ValueObjectHelper
{
    /// <summary>
    /// Retrieves the value of the "Value" property from a value object.
    /// </summary>
    /// <param name="valueObject">The value object containing the "Value" property.</param>
    /// <returns>The Guid value of the "Value" property.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the object does not have a "Value" property of type Guid.
    /// </exception>
    public static Guid? GetValueFromValueObject(object valueObject)
    {
        var property = valueObject.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance);
        if (property is not null && property.PropertyType == typeof(Guid))
        {
            return (Guid?)property.GetValue(valueObject);
        }

        throw new InvalidOperationException("The object does not have a 'Value' property of type Guid.");
    }
}
