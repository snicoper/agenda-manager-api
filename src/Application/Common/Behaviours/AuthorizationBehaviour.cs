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

        if (attributes.Length == 0 || currentUserProvider.IsAuthenticated is false)
        {
            return await next();
        }

        var currentUser = currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            throw new UnauthorizedAccessException();
        }

        var requiredPermissions = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Permissions?.Split(',') ?? [])
            .ToList();

        if (requiredPermissions.Except(currentUser.Permissions).Any())
        {
            var error = Error.Unauthorized("User is forbidden from taking this action");

            var result = ResultBehaviourHelper.CreateResult<TResponse>(error);

            return result;
        }

        var requiredRoles = attributes
            .SelectMany(authorizationAttribute => authorizationAttribute.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Except(currentUser.Roles).Any())
        {
            var error = Error.Unauthorized("User is forbidden from taking this action");

            var result = ResultBehaviourHelper.CreateResult<TResponse>(error);

            return result;
        }

        return await next();
    }
}
