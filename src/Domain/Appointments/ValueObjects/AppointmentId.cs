﻿using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Appointments.ValueObjects;

public class AppointmentId : ValueObject
{
    private AppointmentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static AppointmentId From(Guid value)
    {
        return new AppointmentId(value);
    }

    public static AppointmentId Create()
    {
        return new AppointmentId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
