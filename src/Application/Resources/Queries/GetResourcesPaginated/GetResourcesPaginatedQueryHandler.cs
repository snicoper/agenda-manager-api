using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Resources.Services;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

namespace AgendaManager.Application.Resources.Queries.GetResourcesPaginated;

internal class GetResourcesPaginatedQueryHandler(
    IResourceRepository resourceRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetResourcesPaginatedQuery, ResponseData<GetResourcesPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetResourcesPaginatedQueryResponse>>> Handle(
        GetResourcesPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // Get the resources queryable from the repository.
        var resources = resourceRepository.GetQueryable()
            .Where(r => r.CalendarId == currentUserProvider.GetSelectedCalendarId())
            .AsQueryable();

        // Apply filters to the resources queryable.
        resources = ResourceFilter.ApplyFilters(resources, request.RequestData);

        // Create the response data.
        var responseData = await ResponseData<GetResourcesPaginatedQueryResponse>.CreateAsync(
            source: resources,
            projection: r => new GetResourcesPaginatedQueryResponse(
                ResourceId: r.Id.Value,
                Name: r.Name,
                Description: r.Description,
                TextColor: r.ColorScheme.Text,
                BackgroundColor: r.ColorScheme.Background,
                IsActive: r.IsActive,
                DeactivationReason: r.DeactivationReason,
                CreatedAt: r.CreatedAt),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        // Return the response data.
        return Result.Success(responseData);
    }
}
