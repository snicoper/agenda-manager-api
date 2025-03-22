using Newtonsoft.Json;

namespace AgendaManager.Domain.Appointments.ValueObjects;

public sealed record AppointmentId
{
    [JsonConstructor]
    internal AppointmentId(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);

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
}
