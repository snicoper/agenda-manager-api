using AgendaManager.Application.Common.Http.Filter;
using AgendaManager.Application.Common.Http.OrderBy;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Application.Common.Http;

public class ResponseData<T> : RequestData
    where T : class
{
    public int TotalItems { get; set; }

    public int TotalPages { get; set; } = 1;

    public IEnumerable<T> Items { get; private init; } = new List<T>();

    public static async Task<ResponseData<T>> CreateAsync<TEntity>(
        IQueryable<TEntity> source,
        Func<TEntity, T> projection,
        RequestData request,
        CancellationToken cancellationToken)
    {
        var query = source
            .Filter(request)
            .Ordering(request);

        var totalItems = await query
            .CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => projection(e))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

        var responseData = new ResponseData<T>
        {
            TotalItems = totalItems,
            PageNumber = request.PageNumber,
            TotalPages = totalPages,
            PageSize = request.PageSize,
            Items = items,
            Order = request.Order,
            Filters = request.Filters
        };

        return responseData;
    }
}
