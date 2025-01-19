using System.Text.Json;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Http.Filter;
using AgendaManager.Application.Common.Serializers;
using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;

namespace AgendaManager.Application.ResourceManagement.Resources.Services;

public static class ResourceScheduleFilter
{
    public static IQueryable<ResourceSchedule> ApplyFilters(IQueryable<ResourceSchedule> query, RequestData request)
    {
        if (string.IsNullOrWhiteSpace(request.Filters))
        {
            return query;
        }

        var filters = JsonSerializer
            .Deserialize<List<ItemFilter>>(request.Filters, CustomJsonSerializerOptions.Default())?
            .ToArray() ?? [];

        return filters.Aggregate(query, ApplyFilter);
    }

    private static IQueryable<ResourceSchedule> ApplyFilter(IQueryable<ResourceSchedule> query, ItemFilter filter)
    {
        return filter.PropertyName.ToTitle() switch
        {
            _ => query
        };
    }
}
