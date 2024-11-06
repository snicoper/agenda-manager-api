using AgendaManager.Domain.Common.WekDays.Exceptions;

namespace AgendaManager.Domain.Common.WekDays.Extensions;

public static class WeekDaysExtensions
{
    /// <summary>
    /// Converts the given <see cref="WeekDays" /> to an array of their corresponding day numbers.
    /// </summary>
    /// <param name="weekDays">The input <see cref="WeekDays" />.</param>
    /// <returns>An array of day numbers.</returns>
    /// <remarks>
    /// The array will be ordered by day number.
    /// </remarks>
    public static int[] ToNumberArray(this WeekDays weekDays)
    {
        var days = Enum.GetValues<WeekDays>()
            .Where(
                d => d != WeekDays.None &&
                     d != WeekDays.All &&
                     d != WeekDays.Weekend &&
                     d != WeekDays.Weekdays &&
                     weekDays.HasFlag(d))
            .Select(d => (int)Math.Log2((int)d) + 1)
            .OrderBy(d => d)
            .ToArray();

        return days;
    }

    /// <summary>
    /// Converts an array of day numbers to the corresponding <see cref="WeekDays" /> value.
    /// </summary>
    /// <param name="days">An array of integers representing the days of the week (1 for Monday, 7 for Sunday).</param>
    /// <returns>
    /// A <see cref="WeekDays" /> value representing the combination of days specified in the array.
    /// Returns <see cref="WeekDays.None" /> if the array is null or empty.
    /// </returns>
    /// <exception cref="WeekDaysException">
    /// Thrown when any day number is out of the 1-7 range or if there are duplicate day numbers.
    /// </exception>
    public static WeekDays FromNumberArray(this int[]? days)
    {
        if (days is null || days.Length == 0)
        {
            return WeekDays.None;
        }

        // Validación de rango.
        if (days.Any(d => d is < 1 or > 7))
        {
            throw new WeekDaysException("The day range must be between 1 and 7.");
        }

        // Validación de duplicados.
        if (days.Distinct().Count() != days.Length)
        {
            throw new WeekDaysException("There can be no duplicate days.");
        }

        return days.Aggregate(WeekDays.None, (current, day) => current | (WeekDays)(1 << (day - 1)));
    }
}
