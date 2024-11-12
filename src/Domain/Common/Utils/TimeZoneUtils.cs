using System.Collections.Concurrent;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Common.Utils;

/// <summary>
/// Provides utility methods for handling timezone conversions and validations,
/// with built-in caching for better performance.
/// </summary>
public static class TimeZoneUtils
{
    // Concurrent dictionaries for thread-safe caching of timezone information
    private static readonly ConcurrentDictionary<string, Result<TimeZoneInfo>> _timeZoneCache = new();
    private static readonly ConcurrentDictionary<string, Result<string>> _windowsToIanaCache = new();

    /// <summary>
    /// Retrieves a TimeZoneInfo object from an IANA timezone identifier with built-in caching.
    /// </summary>
    /// <param name="ianaId">The IANA timezone identifier (e.g., "Europe/Madrid").</param>
    /// <returns>
    /// A Result containing either the TimeZoneInfo object if successful,
    /// or an error if the timezone is invalid or conversion fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when ianaId is null.</exception>
    public static Result<TimeZoneInfo> GetTimeZoneInfoFromIana(string ianaId)
    {
        ArgumentNullException.ThrowIfNull(ianaId);

        return _timeZoneCache.GetOrAdd(ianaId, GetTimeZoneInfoInternal);
    }

    /// <summary>
    /// Attempts to convert a Windows timezone ID to its IANA equivalent.
    /// On non-Windows systems, returns the input ID unchanged.
    /// </summary>
    /// <param name="windowsId">The Windows timezone identifier to convert.</param>
    /// <returns>
    /// A Result containing either the IANA timezone ID if successful,
    /// or an error if the conversion fails.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when windowsId is null.</exception>
    public static Result<string> TryConvertWindowsIdToIanaId(string windowsId)
    {
        ArgumentNullException.ThrowIfNull(windowsId);

        return !OperatingSystem.IsWindows()
            ? Result.Success(windowsId)
            : _windowsToIanaCache.GetOrAdd(windowsId, ConvertWindowsIdInternal);
    }

    /// <summary>
    /// Validates whether a given string represents a valid IANA timezone identifier.
    /// </summary>
    /// <param name="timeZone">The timezone identifier to validate.</param>
    /// <returns>True if the timezone is valid, false otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when timeZone is null.</exception>
    public static bool IsValidIanaTimeZone(string timeZone)
    {
        ArgumentNullException.ThrowIfNull(timeZone);

        return GetTimeZoneInfoFromIana(timeZone).IsSuccess;
    }

    /// <summary>
    /// Clears both timezone caches. Primarily used for testing purposes.
    /// </summary>
    internal static void ClearCache()
    {
        _timeZoneCache.Clear();
        _windowsToIanaCache.Clear();
    }

    /// <summary>
    /// Internal method to handle the actual timezone retrieval and conversion logic.
    /// </summary>
    /// <param name="ianaId">The IANA timezone identifier to process.</param>
    /// <returns>A Result containing either the TimeZoneInfo or an error.</returns>
    private static Result<TimeZoneInfo> GetTimeZoneInfoInternal(string ianaId)
    {
        try
        {
            if (!OperatingSystem.IsWindows())
            {
                return Result.Success(TimeZoneInfo.FindSystemTimeZoneById(ianaId));
            }

            return !TimeZoneInfo.TryConvertIanaIdToWindowsId(ianaId, out var windowsId)
                ? Result.Failure<TimeZoneInfo>(Errors.InvalidIana)
                : Result.Success(TimeZoneInfo.FindSystemTimeZoneById(windowsId));
        }
        catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return Result.Failure<TimeZoneInfo>(Errors.InvalidIana);
        }
    }

    /// <summary>
    /// Internal method to handle Windows timezone ID conversion logic.
    /// </summary>
    /// <param name="windowsId">The Windows timezone identifier to convert.</param>
    /// <returns>A Result containing either the IANA identifier or an error.</returns>
    private static Result<string> ConvertWindowsIdInternal(string windowsId)
    {
        try
        {
            return !TimeZoneInfo.TryConvertWindowsIdToIanaId(windowsId, out var ianaId)
                ? Result.Failure<string>(Errors.InvalidWindows(windowsId))
                : Result.Success(ianaId);
        }
        catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
        {
            return Result.Failure<string>(Errors.ConversionError(ex.Message));
        }
    }

    /// <summary>
    /// Contains predefined error messages for consistent error handling throughout the utility.
    /// </summary>
    private static class Errors
    {
        /// <summary>
        /// Error for invalid IANA timezone identifiers.
        /// </summary>
        public static readonly Error InvalidIana = Error.Validation(
            "TimeZone.InvalidIanaId",
            "Invalid IANA time zone ID.");

        /// <summary>
        /// Creates an error for invalid Windows timezone conversions.
        /// </summary>
        public static Error InvalidWindows(string windowsId)
        {
            return Error.Validation(
                "TimeZone.InvalidWindows",
                $"Could not convert Windows timezone '{windowsId}' to IANA format.");
        }

        /// <summary>
        /// Creates an error for general timezone conversion failures.
        /// </summary>
        public static Error ConversionError(string message)
        {
            return Error.Validation(
                "TimeZone.ConversionError",
                $"Error converting timezone: {message}");
        }
    }
}
