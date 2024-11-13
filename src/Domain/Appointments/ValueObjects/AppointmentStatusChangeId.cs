namespace AgendaManager.Domain.Appointments.ValueObjects;

public sealed record AppointmentStatusChangeId
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
}
