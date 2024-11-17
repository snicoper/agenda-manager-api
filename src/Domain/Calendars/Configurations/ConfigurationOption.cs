namespace AgendaManager.Domain.Calendars.Configurations;

public sealed record ConfigurationOption
{
    // Constructor para opciones normales.
    public ConfigurationOption(
        string category,
        string defaultKey,
        string[] availableKeys,
        string description)
    {
        Category = category;
        DefaultKey = defaultKey;
        AvailableKeys = availableKeys;
        Description = description;
        IsUnitValue = false;
    }

    // Constructor para UnitValue.
    public ConfigurationOption(
        string category,
        bool isUnitValue,
        Func<string, bool> validator,
        string description)
    {
        Category = category;
        IsUnitValue = isUnitValue;
        Validator = validator;
        Description = description;
        AvailableKeys = [];
    }

    public string Category { get; }

    public string? DefaultKey { get; }

    public string[] AvailableKeys { get; }

    public bool IsUnitValue { get; }

    public Func<string, bool>? Validator { get; }

    public string Description { get; }
}
