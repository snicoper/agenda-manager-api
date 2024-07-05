using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentValidation;
using MediatR;

namespace AgendaManager.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest> validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommandQuery
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = Error.None();

        foreach (var error in validationResult.Errors)
        {
            errors.AddValidationError(error.PropertyName, error.ErrorMessage);
        }

        var genericArguments = typeof(TResponse).GetGenericArguments();

        if (genericArguments.Length <= 0)
        {
            return (TResponse)errors.ToResult();
        }

        var genericType = typeof(Result<>);
        Type[] types = [genericArguments[0]];
        var create = genericType.MakeGenericType(types);
        var instance = Activator.CreateInstance(create, default, errors) as TResponse;

        return instance ?? throw new InvalidOperationException();
    }
}
