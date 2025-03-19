using Newtonsoft.Json;

namespace AgendaManager.Domain.Appointments.ValueObjects;

public sealed record AppointmentStatusChangeId
{
    [JsonConstructor]
    internal AppointmentStatusChangeId(Guid value)
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
}
