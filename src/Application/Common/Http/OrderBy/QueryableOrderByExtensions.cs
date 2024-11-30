using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.Json;
using AgendaManager.Application.Common.Http.OrderBy.Exceptions;
using AgendaManager.Application.Common.Serializers;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Extensions;

namespace AgendaManager.Application.Common.Http.OrderBy;

public static class QueryableOrderByExtensions
{
    public static IQueryable<TEntity> Ordering<TEntity>(this IQueryable<TEntity> source, RequestData request)
    {
        if (string.IsNullOrWhiteSpace(request.Order))
        {
            return OrderByCreatedIfExists(source);
        }

        var requestItemOrderBy = JsonSerializer.Deserialize<RequestOrderBy>(
            request.Order,
            CustomJsonSerializerOptions.Default());

        if (requestItemOrderBy is null)
        {
            var result = OrderByCreatedIfExists(source);

            return result;
        }

        source = HandleOrderByCommand(source, requestItemOrderBy, OrderByCommandType.OrderBy);

        return source;
    }

    private static IQueryable<TEntity> OrderByCreatedIfExists<TEntity>(IQueryable<TEntity> source)
    {
        var propertyInfo = typeof(TEntity).GetProperty(nameof(AuditableEntity.CreatedAt));

        var result = propertyInfo is not null ? source.OrderBy($"{propertyInfo.Name} DESC") : source;

        return result;
    }

    private static IOrderedQueryable<TEntity> HandleOrderByCommand<TEntity>(
        IQueryable<TEntity> source,
        RequestOrderBy field,
        OrderByCommandType orderByCommandType = OrderByCommandType.ThenBy)
    {
        var fieldName = field.PropertyName.ToUpperFirstLetter();

        var command = orderByCommandType switch
        {
            OrderByCommandType.OrderBy => field.OrderType == OrderType.Asc
                ? QueryableOrderByCommandType.OrderByDescending
                : QueryableOrderByCommandType.OrderBy,
            OrderByCommandType.ThenBy => field.OrderType == OrderType.Desc
                ? QueryableOrderByCommandType.ThenByDescending
                : QueryableOrderByCommandType.ThenBy,
            _ => throw new NotImplementedException()
        };

        source = source.OrderByCommand(fieldName, command);
        var result = (IOrderedQueryable<TEntity>)source;

        return result;
    }

    private static IOrderedQueryable<TEntity> OrderByCommand<TEntity>(
        this IQueryable<TEntity> source,
        string orderByProperty,
        string command)
    {
        var type = typeof(TEntity);
        var orderProperty = type.GetProperty(orderByProperty);
        var properties = orderByProperty.Split('.').Select(name => typeof(TEntity).GetProperty(name)).ToArray();

        if (orderProperty is null && properties.Length > 1)
        {
            orderProperty = properties.FirstOrDefault();
        }

        if (orderProperty is null)
        {
            throw new OrderFieldEntityNotFoundException(type.Name, orderByProperty);
        }

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, orderProperty);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var resultExpression = Expression.Call(
            typeof(Queryable),
            command,
            [type, orderProperty.PropertyType],
            source.Expression,
            Expression.Quote(orderByExpression));

        var result = (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);

        return result;
    }
}
