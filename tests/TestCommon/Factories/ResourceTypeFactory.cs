using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.TestCommon.Factories;

public static class ResourceTypeFactory
{
    public static ResourceType CreateResourceType(
        ResourceTypeId? resourceTypeId = null,
        string? name = null,
        string? description = null,
        ResourceCategory? category = null)
    {
        var resourceType = ResourceType.Create(
            id: resourceTypeId ?? ResourceTypeId.Create(),
            name: name ?? "Resource test",
            description: description ?? "Description resource test",
            category: category ?? ResourceCategory.Staff);

        return resourceType;
    }
}
