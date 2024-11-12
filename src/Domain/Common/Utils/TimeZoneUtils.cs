using System.Collections.Concurrent;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Common.Utils;

public abstract class TimeZoneUtils
{
    private static readonly ConcurrentDictionary<string, Result<TimeZoneInfo>> _timeZoneCache = new();

    public static Result<TimeZoneInfo> GetTimeZoneInfoFromIana(string ianaId)
    {
        var error = Error.Validation("TimeZone.InvalidIanaId", "Invalid IANA time zone ID.");

        var timeZoneInfoResult = _timeZoneCache.GetOrAdd(
            ianaId,
            id =>
            {
                try
                {
                    if (!OperatingSystem.IsWindows())
                    {
                        return Result.Success(TimeZoneInfo.FindSystemTimeZoneById(id));
                    }

                    return !TimeZoneInfo.TryConvertIanaIdToWindowsId(id, out var windowsId)
                        ? Result.Failure<TimeZoneInfo>(error)
                        : Result.Success(TimeZoneInfo.FindSystemTimeZoneById(windowsId));
                }
                catch (Exception ex) when (ex is TimeZoneNotFoundException or InvalidTimeZoneException)
                {
                    return Result.Failure<TimeZoneInfo>(error);
                }
            });

        return timeZoneInfoResult;
    }

    public static bool IsValidIanaTimeZone(string timeZone)
    {
        return GetTimeZoneInfoFromIana(timeZone).IsSuccess;
    }
}
