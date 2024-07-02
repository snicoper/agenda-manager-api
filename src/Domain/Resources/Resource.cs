using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources;

public class Resource(ResourceId id, string name) : AuditableEntity
{
    public ResourceId Id { get; } = id;

    public string Name { get; private set; } = name;

    public Result AddName(string name)
    {
        AddDomainEvent(new ResourceNameAddedDomainEvent(Id));

        return name == "hello" ? ResourceErrors.ErrorName : Result.Success();
    }
}
