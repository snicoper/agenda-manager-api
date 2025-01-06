namespace AgendaManager.Application.Resources.Queries.GetResourceById;

public record GetResourceByIdQueryResponse(
    Guid ResourceId,
    string Name,
    string Description,
    string TextColor,
    string BackgroundColor,
    bool IsActive,
    string? DeactivationReason,
    DateTimeOffset CreatedAt);
