using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(
    ILogger<TResponse> logger,
    IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAppBaseRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validator is null)
        {
            return await next();
        }

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errorResult = Error.None();

        errorResult = validationResult.Errors.Aggregate(
            errorResult,
            (current, error) => current.AddValidationError(error.PropertyName, error.ErrorMessage));

        logger.LogWarning(
            "An error occurred during validation {Request} with errors: {@ValidationErrors}",
            request,
            errorResult.ToDictionary());

        var result = ResultBehaviourHelper.CreateResult<TResponse>(errorResult);

        return result;
    }
}
