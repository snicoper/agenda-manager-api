using System.Text.Json;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Http.Filter;
using AgendaManager.Application.Common.Serializers;
using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Users.Services;

public static class UserFilter
{
    public static IQueryable<User> ApplyFilters(IQueryable<User> query, RequestData request)
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

    private static IQueryable<User> ApplyFilter(IQueryable<User> query, ItemFilter filter)
    {
        return filter.PropertyName switch
        {
            nameof(User.Email) => ApplyEmailFilter(query, filter),
            _ => query
        };
    }

    private static IQueryable<User> ApplyEmailFilter(IQueryable<User> query, ItemFilter filter)
    {
        return filter.RelationalOperator switch
        {
            FilterOperator.Contains => query.Where(u => ((string)u.Email).ToLower().Contains(filter.Value.ToLower())),
            FilterOperator.EqualTo => query.Where(u => u.Email.Value == filter.Value),
            _ => query
        };
    }
}
