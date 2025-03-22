using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using AgendaManager.Application.Common.Serializers;
using AgendaManager.Domain.Common.Extensions;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Application.Common.Http.Filter;

public static class QueryableFilterExtensions
{
    public static IQueryable<TEntity> Filter<TEntity>(this IQueryable<TEntity> source, RequestData request)
    {
        if (string.IsNullOrWhiteSpace(request.Filters))
        {
            return source;
        }

        var itemsFilter = JsonSerializer
            .Deserialize<List<ItemFilter>>(request.Filters, CustomJsonSerializerOptions.Default())?
            .ToArray() ?? [];

        // Filtramos los Value Objects.
        var validFilters = itemsFilter
            .Where(
                filter =>
                {
                    var propertyInfo = typeof(TEntity).GetProperty(PropertyNameToUpper(filter.PropertyName));

                    return propertyInfo is not null && !IsValueObject(propertyInfo.PropertyType);
                })
            .ToArray();

        if (validFilters.Length == 0)
        {
            return source;
        }

        var filterValues = validFilters
            .Select(
                filter => filter.RelationalOperator == FilterOperator.Contains
                    ? filter.Value.ToLower()
                    : filter.Value)
            .ToDynamicArray();

        // Los FilterOperator.And se separan en un nuevo Where o dará problemas con
        // la precedencia a la hora de anidar las condiciones con los FilterOperator.Or.
        // El resto de FilterOperator, no da problemas estando en un solo Where.
        var query = new StringBuilder();

        for (var position = 0; position < itemsFilter.Length; position++)
        {
            var itemFilter = itemsFilter[position];
            var logicalOperator = !string.IsNullOrWhiteSpace(itemFilter.LogicalOperator)
                ? FilterOperator.GetLogicalOperator(itemFilter.LogicalOperator)
                : string.Empty;

            if (logicalOperator == FilterOperator.GetLogicalOperator(FilterOperator.And))
            {
                var andQuery = new StringBuilder();
                var filter = itemsFilter[position] with { LogicalOperator = string.Empty };
                andQuery = ComposeQuery(filter, andQuery, position);

                source = source.Where(andQuery.ToString(), filterValues);
            }
            else
            {
                query = ComposeQuery(itemsFilter[position], query, position);
            }
        }

        source = source.Where(query.ToString(), filterValues);

        return source;
    }

    private static StringBuilder ComposeQuery(ItemFilter filter, StringBuilder query, int valuePosition)
    {
        var propertyName = PropertyNameToUpper(filter.PropertyName);
        var relationalOperator = FilterOperator.GetRelationalOperator(filter.RelationalOperator);

        var logicalOperator = !string.IsNullOrWhiteSpace(filter.LogicalOperator)
            ? FilterOperator.GetLogicalOperator(filter.LogicalOperator)
            : string.Empty;

        // Comprobar si es un operador de string o lógico.
        var filterResult = filter.RelationalOperator != FilterOperator.Contains
                           && filter.RelationalOperator != FilterOperator.StartsWith
                           && filter.RelationalOperator != FilterOperator.EndsWith
            ? $"{logicalOperator} {propertyName} {relationalOperator} @{valuePosition}"
            : $"{logicalOperator} {string.Format(propertyName + relationalOperator, valuePosition)}";

        query.Append(filterResult);

        return query;
    }

    private static string PropertyNameToUpper(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            return string.Empty;
        }

        var propertyNameParts = propertyName.Split('.');

        var propertyNameResult = propertyNameParts
            .Aggregate(string.Empty, (current, part) => current + $"{part.ToUpperFirstLetter()}.");

        return propertyNameResult.TrimEnd('.');
    }

    /// <summary>
    /// Mientras EF Core no soporte Contains con los ValueObject,
    /// omitir filtros que contengan ValueObject.
    /// </summary>
    private static bool IsValueObject(Type type)
    {
        return typeof(IValueObject).IsAssignableFrom(type);
    }
}
