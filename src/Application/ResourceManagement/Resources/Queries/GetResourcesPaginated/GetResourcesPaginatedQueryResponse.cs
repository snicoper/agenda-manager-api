namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourcesPaginated;

public record GetResourcesPaginatedQueryResponse(
    Guid ResourceId,
    string Name,
    string Description,
    string TextColor,
    string BackgroundColor,
    bool IsActive,
    string? DeactivationReason,
    DateTimeOffset CreatedAt);
