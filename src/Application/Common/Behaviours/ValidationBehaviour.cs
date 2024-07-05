using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IValidator<TRequest> validator, ILogger<TResponse> logger)
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

        logger.LogWarning("Validation errors in request {Request}: {@ValidationErrors}", request, errors.ValidationErrors.Values);

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
