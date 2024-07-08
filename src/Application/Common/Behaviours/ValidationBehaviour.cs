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

        var errors = Error.None();

        foreach (var error in validationResult.Errors)
        {
            errors.AddValidationError(error.PropertyName, error.ErrorMessage);
        }

        logger.LogWarning(
            "An error occurred during validation {Request} with errors: {@ValidationErrors}",
            request,
            errors.ToDictionary());

        return ResultBehaviourHelper.CreateResult<TResponse>(errors);
    }
}
