namespace AgendaManager.Application.Common.Exceptions;

public class AuthorizationRequiredException : Exception
{
    public AuthorizationRequiredException()
        : base("Permission required for this action.")
    {
    }
}
