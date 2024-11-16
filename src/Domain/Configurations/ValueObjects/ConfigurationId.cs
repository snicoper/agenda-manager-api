namespace AgendaManager.Domain.Configurations.ValueObjects;

public record ConfigurationId
{
    private ConfigurationId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ConfigurationId From(Guid value)
    {
        return new ConfigurationId(value);
    }

    public static ConfigurationId Create()
    {
        return new ConfigurationId(Guid.NewGuid());
    }
}
