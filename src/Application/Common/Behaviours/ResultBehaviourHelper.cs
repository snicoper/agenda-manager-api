using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Common.Behaviours;

public static class ResultBehaviourHelper
{
    public static TResponse CreateResult<TResponse>(Error errors)
        where TResponse : Result
    {
        var genericArguments = typeof(TResponse).GetGenericArguments();

        if (genericArguments.Length <= 0)
        {
            return (TResponse)errors.ToResult();
        }

        var genericType = typeof(Result<>);
        Type[] types = [genericArguments[0]];
        var create = genericType.MakeGenericType(types);
        var instance = Activator.CreateInstance(create, default, errors) as TResponse;

        return instance ?? throw new InvalidOperationException("Failed to create Result<T>");
    }
}
