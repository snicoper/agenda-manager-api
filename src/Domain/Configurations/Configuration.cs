using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Configurations.Events;
using AgendaManager.Domain.Configurations.ValueObjects;

namespace AgendaManager.Domain.Configurations;

public sealed class Configuration : AggregateRoot
{
    private Configuration()
    {
    }

    private Configuration(
        ConfigurationId configurationId,
        string category,
        string subcategory,
        string key,
        string description)
    {
        ConfigurationId = configurationId;
        Category = category;
        SubCategory = subcategory;
        Key = key;
        Description = description;
    }

    public ConfigurationId ConfigurationId { get; } = null!;

    public string Category { get; private set; } = default!;

    public string SubCategory { get; private set; } = default!;

    public string Key { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public Configuration Create(
        ConfigurationId configurationId,
        string category,
        string subcategory,
        string key,
        string description)
    {
        Configuration configuration = new(configurationId, category, subcategory, key, description);

        configuration.AddDomainEvent(new ConfigurationCreatedDomainEvent(configurationId));

        return configuration;
    }
}
