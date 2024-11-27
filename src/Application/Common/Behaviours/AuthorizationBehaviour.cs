using System.Reflection;
using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Models;
using AgendaManager.Application.Users.Services;
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

        if (currentUser is null)
        {
            return ErrorUnauthorizedResult();
        }

        var permissionBasedAuthorization = PermissionBasedAuthorization(attributes, currentUser);

        if (permissionBasedAuthorization is not null && permissionBasedAuthorization.IsFailure)
        {
            return permissionBasedAuthorization;
        }

        var roleBasedAuthorization = RoleBasedAuthorization(attributes, currentUser);

        if (roleBasedAuthorization is not null && roleBasedAuthorization.IsFailure)
        {
            return roleBasedAuthorization;
        }

        return await next();
    }

    private static TResponse? PermissionBasedAuthorization(AuthorizeAttribute[] attributes, CurrentUser currentUser)
    {
        var requiredPermissions = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        return !requiredPermissions.Except(currentUser.Permissions).Any() ? null : ErrorUnauthorizedResult();
    }

    private static TResponse? RoleBasedAuthorization(AuthorizeAttribute[] attributes, CurrentUser currentUser)
    {
        var requiredRoles = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        return !requiredRoles.Except(currentUser.Roles).Any() ? null : ErrorUnauthorizedResult();
    }

    private static TResponse ErrorUnauthorizedResult()
    {
        var error = Error.Unauthorized(description: "User is Unauthorized from taking this action");

        var result = ResultBehaviourHelper.CreateResult<TResponse>(error);

        return result;
    }
}
