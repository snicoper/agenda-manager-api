using System.Reflection;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Security;
using AgendaManager.Domain.Users.Persistence;
using MediatR;

namespace AgendaManager.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse>(ICurrentUserService currentUserService, IUsersRepository usersRepository)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
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

        if (currentUserService.Id == Guid.NewGuid())
        {
            throw new UnauthorizedAccessException();
        }

        await RoleBasedAuthorization(attributes, currentUserService.Id);
        await PolicyBasedAuthorization(attributes, currentUserService.Id);

        return await next();
    }

    private async Task RoleBasedAuthorization(IEnumerable<AuthorizeAttribute> attributes, Guid userId)
    {
        var authorizeAttributesWithRoles = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));
        var attributesWithRoles = authorizeAttributesWithRoles as AuthorizeAttribute[] ?? authorizeAttributesWithRoles.ToArray();

        if (attributesWithRoles.Length == 0)
        {
            return;
        }

        var authorized = false;

        foreach (var roles in attributesWithRoles.Select(a => a.Roles.Split(',')))
        {
            foreach (var role in roles)
            {
                var isInRole = await usersRepository.IsInRoleAsync(userId, role.Trim());

                if (!isInRole)
                {
                    continue;
                }

                authorized = true;
                break;
            }
        }

        if (!authorized)
        {
            throw new ForbiddenAccessException();
        }
    }

    private async Task PolicyBasedAuthorization(IEnumerable<AuthorizeAttribute> attributes, Guid userId)
    {
        var authorizeAttributesWithPolicies = attributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

        var attributesWithPolicies =
            authorizeAttributesWithPolicies as AuthorizeAttribute[] ?? authorizeAttributesWithPolicies.ToArray();

        if (attributesWithPolicies.Length == 0)
        {
            return;
        }

        foreach (var policy in attributesWithPolicies.Select(a => a.Policy))
        {
            var authorized = await usersRepository.AuthorizeAsync(userId, policy);

            if (!authorized)
            {
                throw new ForbiddenAccessException();
            }
        }
    }
}
