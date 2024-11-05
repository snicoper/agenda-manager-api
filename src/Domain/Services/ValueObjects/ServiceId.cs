using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Services.ValueObjects;

public class ServiceId : ValueObject
{
    private ServiceId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ServiceId From(Guid value)
    {
        return new ServiceId(value);
    }

    public static ServiceId Create()
    {
        return new ServiceId(Guid.NewGuid());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
