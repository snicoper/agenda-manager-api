using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class ResourceTypeFactory
{
    public static ResourceType CreateResourceType(
        ResourceTypeId? resourceTypeId = null,
        string? name = null,
        string? description = null,
        RoleId? roleId = null)
    {
        var resourceType = ResourceType.Create(
            id: resourceTypeId ?? ResourceTypeId.Create(),
            name: name ?? "Resource test",
            description: description ?? "Description resource test",
            roleId: roleId);

        return resourceType;
    }
}
