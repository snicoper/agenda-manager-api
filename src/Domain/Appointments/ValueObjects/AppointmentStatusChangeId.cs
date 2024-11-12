using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Appointments.ValueObjects;

public sealed class AppointmentStatusChangeId : ValueObject
{
    private AppointmentStatusChangeId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public Guid Value { get; }

    public static AppointmentStatusChangeId From(Guid value)
    {
        return new AppointmentStatusChangeId(value);
    }

    public static AppointmentStatusChangeId Create()
    {
        return new AppointmentStatusChangeId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
