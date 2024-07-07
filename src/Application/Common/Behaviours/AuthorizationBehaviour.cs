using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Security;
using AgendaManager.Domain.Common.Responses;
using MediatR;

namespace AgendaManager.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(ICurrentUserProvider currentUserProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAppBaseRequest
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
        var attributes = authorizeAttributes as AuthorizeAttribute[] ?? authorizeAttributes.ToArray();

        if (attributes.Length == 0)
        {
            return await next();
        }

        var currentUser = currentUserProvider.GetCurrentUser();

        if (currentUser.Id.Value == Guid.NewGuid())
        {
            throw new UnauthorizedAccessException();
        }

        var requiredPermissions = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        if (requiredPermissions.Except(currentUser.Permissions).Any())
        {
            var error = Error.Unauthorized(description: "User is forbidden from taking this action");

            return CreateResult(error);
        }

        var requiredRoles = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            var error = Error.Unauthorized(description: "User is forbidden from taking this action");

            return CreateResult(error);
        }

        return await next();
    }

    private static TResponse CreateResult(Error errors)
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
