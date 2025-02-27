﻿using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record IanaTimeZone
{
    private IanaTimeZone(string value)
    {
        GuardAgainstInvalidTimeZone(value);

        Value = value;
    }

    public string Value { get; }

    public TimeZoneInfo Info { get; private set; } = null!;

    public static IanaTimeZone FromIana(string value)
    {
        return new IanaTimeZone(value);
    }

    public static IanaTimeZone FromTimeZoneInfo(TimeZoneInfo timeZoneInfo)
    {
        ArgumentNullException.ThrowIfNull(timeZoneInfo);

        if (!OperatingSystem.IsWindows())
        {
            return new IanaTimeZone(timeZoneInfo.Id);
        }

        var ianaIdResult = TimeZoneUtils.TryConvertWindowsIdToIanaId(timeZoneInfo.Id);

        if (ianaIdResult is { IsSuccess: false, Value: not null })
        {
            throw new InvalidTimeZoneException(ianaIdResult.Error?.FirstError()?.Description);
        }

        return new IanaTimeZone(ianaIdResult.Value!);
    }

    private void GuardAgainstInvalidTimeZone(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var timeZoneInfoResult = TimeZoneUtils.GetTimeZoneInfoFromIana(value);

        if (timeZoneInfoResult.IsFailure)
        {
            throw new DomainException(timeZoneInfoResult.Error?.FirstError()?.Description!);
        }

        Info = timeZoneInfoResult.Value!;
    }
}
