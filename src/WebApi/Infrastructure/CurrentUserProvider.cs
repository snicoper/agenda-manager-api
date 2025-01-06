using System.Security.Claims;
using AgendaManager.Application.Users.Models;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.WebApi.Exceptions;

namespace AgendaManager.WebApi.Infrastructure;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public CurrentUser? GetCurrentUser()
    {
        if (IsAuthenticated is false)
        {
            return null;
        }

        var userId = GetClaimValues(CustomClaimType.Id)
            .Select(Guid.Parse)
            .First();

        var permissions = GetClaimValues(CustomClaimType.Permissions);
        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(UserId.From(userId), roles, permissions);
    }

    public CalendarId GetSelectedCalendarId()
    {
        if (httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("X-Calendar-Id", out var calendarId) is not
            true)
        {
            throw new CalendarNotSelectedException("No calendar selected in request headers");
        }

        try
        {
            return CalendarId.From(Guid.Parse(calendarId!));
        }
        catch (FormatException)
        {
            throw new InvalidCalendarIdException($"Invalid calendar id format: {calendarId}");
        }
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        var claim = httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList()
            .AsReadOnly();

        return claim;
    }
}
